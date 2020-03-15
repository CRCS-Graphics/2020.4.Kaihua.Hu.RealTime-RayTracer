using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "RT_Material/Create RT_Material")]
public class RT_Material_Instance : ScriptableObject
{
    public Color color = new Color();

    public myVector specular;
    public myVector diffuse;
    public myVector reflectness;

    [Range(1, 100)]
    public uint shininess = 5;
}


[System.Serializable]
public struct myVector
{
    [Range(0, 1)]
    public float R;
    [Range(0, 1)]
    public float G;
    [Range(0, 1)]
    public float B;
}