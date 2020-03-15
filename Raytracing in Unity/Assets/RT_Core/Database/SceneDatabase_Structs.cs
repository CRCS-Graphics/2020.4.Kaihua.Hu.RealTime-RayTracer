using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RT_Core : MonoBehaviour
{
    public struct RTSphere
    {
        public Matrix4x4 worldToLocal;
        public uint matIndex;
    };

    public struct DirectionalLight
    {
        public Vector3 color;
        public Vector3 direction;
    };

    public struct PointLight
    {
        public Vector3 color;
        public Vector3 position;
        public float range;
    };

    public struct RTMaterial
    {
        public Vector3 color;
        public Vector3 specular;
        public Vector3 diffuse;
        public Vector3 reflectness;
        public uint shininess;
    }

    public struct RTObject
    {
        public Matrix4x4 worldToLocal;
        public int startIndex;
        public int size;
        public uint matIndex;
    }

    public struct ObjectMeshIndex
    {
        public int startIndex;
        public int size;
    }

    public struct RTTriangle
    {
        public Vector3 v0;
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 normal;
        public uint matIndex;
    }

    public struct RTBVHNode
    {
        public uint isLeaf;
        public Vector3 boundingBoxMin;
        public Vector3 boundingBoxMax;
        public uint subTreeCount;
        public uint startIndex;
        public uint triangleCount;
    }
}
