using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RT_Core
{
    private List<Triangle> KDtriangles;
    private List<RTBVHNode> BVHTreeList;
    private List<RTTriangle> triangleList;

    public void initBVHTreeDataBase()
    {
        directionalLights = new List<DirectionalLight>();
        pointLights = new List<PointLight>();
        collectLightData();

        materials = new List<RTMaterial>();
        materialMap = new Dictionary<string, uint>();
        KDtriangles = new List<Triangle>();

        collectDataForBVHTree();

        BVHTreeList = new List<RTBVHNode>();
        triangleList = new List<RTTriangle>();
        BVHTree tree = new BVHTree(KDtriangles, useBVHTreeStructure);
        BVHNode root = tree.root;
        BVHTreeToArrayDfs(root, BVHTreeList, triangleList, 0);  
    }
    public void LasyUpdateBVHTreeDataBase()
    {
        directionalLights = new List<DirectionalLight>();
        pointLights = new List<PointLight>();
        collectLightData();

        if (transformsChanged())
        {
            materials = new List<RTMaterial>();
            materialMap = new Dictionary<string, uint>();
            KDtriangles = new List<Triangle>();

            collectDataForBVHTree();

            BVHTreeList = new List<RTBVHNode>();
            triangleList = new List<RTTriangle>();
            BVHTree tree = new BVHTree(KDtriangles, useBVHTreeStructure);
            BVHNode root = tree.root;
            BVHTreeToArrayDfs(root, BVHTreeList, triangleList, 0);
        }
    }

    public bool transformsChanged()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject primitive in gameObjects)
        {
            if (primitive.transform.hasChanged)
            {
                primitive.transform.hasChanged = false;
                return true;
            }
        }
        return false;
    }

    public void collectDataForBVHTree()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject primitive in gameObjects)
        {
            uint materialIndex = collectMaterial(primitive);
            Mesh mesh = primitive.GetComponent<MeshFilter>().mesh;

            int[] triangles = mesh.triangles;
            Vector3[] normals = mesh.normals;
            Vector3[] vertices = mesh.vertices;

            // collect each triangle and convert it to world space
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Matrix4x4 localToWorld = primitive.transform.localToWorldMatrix;
                Matrix4x4 worldToLocal = primitive.transform.worldToLocalMatrix;
                Matrix4x4 t = worldToLocal.transpose;

                Vector3 normal0 = normals[triangles[i]];
                Vector3 normal1 = normals[triangles[i + 1]];
                Vector3 normal2 = normals[triangles[i + 2]];

                Vector3 normal3 = (normal0 + normal1 + normal2) / 3;
                Vector3 normal = t.MultiplyVector(normal3);
                normal.Normalize();



                Vector3 vertice0 = localToWorld.MultiplyPoint(vertices[triangles[i]]);
                Vector3 vertice1 = localToWorld.MultiplyPoint(vertices[triangles[i + 1]]);
                Vector3 vertice2 = localToWorld.MultiplyPoint(vertices[triangles[i + 2]]);

                Triangle temp = new Triangle(vertice0, vertice1, vertice2, normal, materialIndex);
                KDtriangles.Add(temp);
            }
        }
    }

    public int BVHTreeToArrayDfs(BVHNode node, List<RTBVHNode> BVHTreeList, List<RTTriangle> triangleList, int listIndex)
    {
        if (node == null)
        {
            return 0;
        }

        if (node.left == null && node.right == null)
        {
            RTBVHNode tempNode = new RTBVHNode()
            {
                isLeaf = 1,
                startIndex = (uint)triangleList.Count,
                triangleCount = (uint)node.triangles.Count,
                subTreeCount = 0,
            };

            BVHTreeList.Add(tempNode);

            foreach (Triangle tri in node.triangles)
            {
                RTTriangle tempTriangle = new RTTriangle()
                {
                    v0 = tri.v0,
                    v1 = tri.v1,
                    v2 = tri.v2,
                    normal = tri.normal,
                    matIndex = tri.matIndex,
                };
                triangleList.Add(tempTriangle);
            }
            return 1;
        }

        RTBVHNode tempNode2 = new RTBVHNode();
        BVHTreeList.Add(tempNode2);
        int index = BVHTreeList.Count - 1;

        int leftCount = BVHTreeToArrayDfs(node.left, BVHTreeList, triangleList, listIndex + 1);
        int rightCount = BVHTreeToArrayDfs(node.right, BVHTreeList, triangleList, listIndex + 1);

        int count = leftCount + rightCount;

        RTBVHNode tempNode1 = new RTBVHNode()
        {
            isLeaf = 0,
            boundingBoxMin = node.bbox.min,
            boundingBoxMax = node.bbox.max,
            subTreeCount = (uint)count,
        };
        BVHTreeList[index] = tempNode1;


        return count + 1;
    }

}
