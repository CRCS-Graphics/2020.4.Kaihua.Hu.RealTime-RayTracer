using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHTree { 
    public BVHNode root;

    public BVHTree (List<Triangle> triangles, bool useBVHTree)
    {
        if(useBVHTree)
        {
            root = build(triangles, 0);
        } else
        {
            root = new BVHNode();
            root.triangles = triangles;
            root.left = null;
            root.right = null;
            root.bbox = new BoundingBox(triangles);
        }
    }

    public BVHNode build(List<Triangle> triangles, int depth)
    {
        if(triangles.Count == 0)
        {
            return null;
        }

        BVHNode node = new BVHNode();

        node.triangles = triangles;
        node.left = null;
        node.right = null;
        node.bbox = new BoundingBox(triangles);


        if (triangles.Count <= 5)
        {
            return node;
        }

        // find cut plane
        Vector3 mid = new Vector3(0f, 0f, 0f);
        float div = 1.0f / triangles.Count;
        foreach (Triangle t in triangles)
        {
            mid = mid + (t.center * div);
        }

        List<Triangle> leftTriangles = new List<Triangle>();
        List<Triangle> rightTriangles = new List<Triangle>();

        char axis = node.bbox.getLongestAxis();

        foreach (Triangle t in triangles)
        {
            switch (axis)
            {
                case 'x':
                    if (mid.x < t.center.x)
                    {
                        rightTriangles.Add(t);
                    }
                    else
                    {
                        leftTriangles.Add(t);
                    }
                    break;
                case 'y':
                    if (mid.y < t.center.y)
                    {
                        rightTriangles.Add(t);
                    }
                    else
                    {
                        leftTriangles.Add(t);
                    }
                    break;
                case 'z':
                    if (mid.z < t.center.z)
                    {
                        rightTriangles.Add(t);
                    }
                    else
                    {
                        leftTriangles.Add(t);
                    }
                    break;
            }
        }

        if (depth < 12)
        {
            node.left = build(leftTriangles, depth + 1);
            node.right = build(rightTriangles, depth + 1);
        }

        return node;
    }

}
