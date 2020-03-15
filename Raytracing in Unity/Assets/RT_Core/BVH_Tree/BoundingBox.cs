using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RT_Core;

public class BoundingBox
{
    public Vector3 max;
    public Vector3 min;

    public BoundingBox()
    {
        min = new Vector3();
        max = new Vector3();
    }

    public BoundingBox(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        min.x = Mathf.Min(point0.x, point1.x, point2.x);
        min.y = Mathf.Min(point0.y, point1.y, point2.y);
        min.z = Mathf.Min(point0.z, point1.z, point2.z);

        max.x = Mathf.Max(point0.x, point1.x, point2.x);
        max.y = Mathf.Max(point0.y, point1.y, point2.y);
        max.z = Mathf.Max(point0.z, point1.z, point2.z);
    }

    public BoundingBox(List<Triangle> triangles)
    {
        min = new Vector3(triangles[0].v0.x, triangles[0].v0.y, triangles[0].v0.z);
        max = new Vector3(min.x, min.y, min.z);
        foreach (Triangle t in triangles)
        {
            addTriangle(t);
        }
        //drawBox();
    }

    public void addTriangle(Triangle t)
    {
        Vector3 point0 = t.v0;
        Vector3 point1 = t.v1;
        Vector3 point2 = t.v2;

        min.x = Mathf.Min(point0.x, point1.x, point2.x, min.x);
        min.y = Mathf.Min(point0.y, point1.y, point2.y, min.y);
        min.z = Mathf.Min(point0.z, point1.z, point2.z, min.z);

        max.x = Mathf.Max(point0.x, point1.x, point2.x, max.x);
        max.y = Mathf.Max(point0.y, point1.y, point2.y, max.y);
        max.z = Mathf.Max(point0.z, point1.z, point2.z, max.z);
    }

    public void drawBox()
    {
        float xLength = getXLength();
        float yLength = getYLength();
        float zLength = getZLength();

        Vector3 v1 = new Vector3(max.x - xLength, max.y, max.z);
        Vector3 v2 = new Vector3(max.x, max.y - yLength, max.z);
        Vector3 v3 = new Vector3(max.x, max.y, max.z - zLength);

        Vector3 v4 = new Vector3(min.x + xLength, min.y, min.z);
        Vector3 v5 = new Vector3(min.x, min.y + yLength, min.z);
        Vector3 v6 = new Vector3(min.x, min.y, min.z + zLength);

        Debug.DrawLine(max, v1, Color.green, 10000);
        Debug.DrawLine(max, v2, Color.green, 10000);
        Debug.DrawLine(max, v3, Color.green, 10000);

        Debug.DrawLine(min, v4, Color.green, 10000);
        Debug.DrawLine(min, v5, Color.green, 10000);
        Debug.DrawLine(min, v6, Color.green, 10000);

        Debug.DrawLine(v4, v2, Color.green, 10000);
        Debug.DrawLine(v4, v3, Color.green, 10000);
        Debug.DrawLine(v5, v3, Color.green, 10000);
        Debug.DrawLine(v5, v1, Color.green, 10000);
        Debug.DrawLine(v6, v2, Color.green, 10000);
        Debug.DrawLine(v6, v1, Color.green, 10000);
    }

        public Vector3 getCenter()
    {
        float x = (min.x + max.x) * 0.5f;
        float y = (min.y + max.y) * 0.5f;
        float z = (min.z + max.z) * 0.5f;
        Vector3 res = new Vector3(x, y, z);
        return res;
    }

    public bool isEmpty()
    {
        return (min.x > max.x) || (min.y > max.y) || (min.z > max.z);
    }

    public float getXLength()
    {
        return max.x - min.x;
    }

    public float getYLength()
    {
        return max.y - min.y;
    }

    public float getZLength()
    {
        return max.z - min.z;
    }

    public char getLongestAxis()
    {
        char res;

        float x = getXLength();
        float y = getYLength();
        float z = getZLength();

        if (x >= y)
        {
            res = 'x';
            if (x < z)
            {
                res = 'z';
            }
        } else
        {
            res = 'y';
            if (y < z)
            {
                res = 'z';
            }
        }

        return res;
    }
}
