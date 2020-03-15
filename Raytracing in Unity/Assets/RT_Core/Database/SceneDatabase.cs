using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public partial class RT_Core
{
    [Range(1, 20)]
    public int SamplesPerPixel = 1;
    [Range(1, 10)]
    public int MaxRayGeneration = 1;

    public bool enableShadow = true;

    public bool useBVHTreeStructure = true;

    public bool lasyUpdateScene = true;

    private List<float> sampleOffset = new List<float>();

    // Unity 
    //----------------------------------------------------------------------------------------

    // Sphere
    //----------------------------------------------------------------------------------------
    private List<RTSphere> spheres;

    // Lights
    //----------------------------------------------------------------------------------------
    private List<DirectionalLight> directionalLights;
    private List<PointLight> pointLights;

    // Matrial
    //----------------------------------------------------------------------------------------
    private List<RTMaterial> materials;
    private Dictionary<string, uint> materialMap;


    public void collectLightData()
    {
        Light[] lights = GameObject.FindObjectsOfType(typeof(Light)) as Light[];

        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                DirectionalLight directionalLight = new DirectionalLight()
                {
                    color = new Vector3(light.color.r, light.color.g, light.color.b),
                    direction = light.transform.forward
                };
                directionalLights.Add(directionalLight);
            } else if (light.type == LightType.Point)
            {
                PointLight pointLight = new PointLight()
                {
                    color = new Vector3(light.color.r, light.color.g, light.color.b),
                    position = light.transform.position,
                    range = light.range
                };
               pointLights.Add(pointLight);
            }

        }
    }

    public uint collectMaterial(GameObject primitive)
    {
        RT_Material temp = primitive.GetComponent<RT_Material>();
        if (temp != null)
        {
            RT_Material_Instance instance = temp.RT_material;

            if(instance == null)
            {
                return 0;
            }

            string matName = instance.name;

            if (materialMap.ContainsKey(matName))
            {
                return materialMap[matName];
            }

            RTMaterial mat = new RTMaterial()
            {
                color = new Vector3(instance.color.r, instance.color.g, instance.color.b),
                specular = new Vector3(instance.specular.R, instance.specular.G, instance.specular.B),
                diffuse = new Vector3(instance.diffuse.R, instance.diffuse.G, instance.diffuse.B),
                reflectness = new Vector3(instance.reflectness.R, instance.reflectness.G, instance.reflectness.B),
                shininess = instance.shininess
            };

            uint index = (uint)materials.Count;
            materials.Add(mat);
            materialMap.Add(matName, index);
            return index;
        }

        return 0;
    }


}

