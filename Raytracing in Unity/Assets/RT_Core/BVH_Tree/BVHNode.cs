using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RT_Core;

public class BVHNode 
{
    public BoundingBox bbox;
    public BVHNode left;
    public BVHNode right;
    public List<Triangle> triangles;

    public BVHNode()
    {

    }
}
