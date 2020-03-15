using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public Vector3 v0;
    public Vector3 v1;
    public Vector3 v2;
    public Vector3 center;
    public Vector3 normal;
    public uint matIndex;

    public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
        center = getCenter();
    }

    public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 normal, uint matIndex)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
        this.normal = normal;
        this.matIndex = matIndex;
        center = getCenter();

    }

    public Vector3 getCenter()
    {
        return ((v0 + v1) * 0.5f + v2) * 0.5f;
    } 
}
