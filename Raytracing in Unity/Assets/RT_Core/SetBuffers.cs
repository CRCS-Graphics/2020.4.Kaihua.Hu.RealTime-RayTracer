using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RT_Core : MonoBehaviour
{
    private ComputeBuffer SampleBuffer;
    private ComputeBuffer MaterialBuffer;
    private ComputeBuffer DirectionLightBuffer;
    private ComputeBuffer PointLightBuffer;

    private void initBuffers()
    {
        int main = RT_MainShader.FindKernel("RTmain");
        SetCameraAndSkybox(main);

        // Material
        loadBuffer(materials, ref MaterialBuffer, sizeof(int) + sizeof(float) * 12);
        SetGlobalBuffer("_Materials", MaterialBuffer);

        //Lights
        loadBuffer(directionalLights, ref DirectionLightBuffer, sizeof(float) * 6);   
        SetGlobalBuffer("_DirectionalLights", DirectionLightBuffer);

        loadBuffer(pointLights, ref PointLightBuffer, sizeof(float) * 7);
        SetGlobalBuffer("_PointLights", PointLightBuffer);

    }

    private void SetCameraAndSkybox(int index)
    {
        RT_MainShader.SetMatrix("_CameraToWorld", MainCamera.cameraToWorldMatrix);
        RT_MainShader.SetMatrix("_CameraInverseProjection", MainCamera.projectionMatrix.inverse);
        RT_MainShader.SetTexture(index, "_SkyboxTexture", SkyboxTexture);

        RT_MainShader.SetInt("_MaxGeneration", MaxRayGeneration);

        if (sampleOffset.Count == SamplesPerPixel)
        {
            return;
        }

        sampleOffset.Clear();
        for (int i = 0; i < SamplesPerPixel; i ++)
        {
            sampleOffset.Add(Random.value);
        }

        loadBuffer(sampleOffset, ref SampleBuffer, sizeof(float) );
        SetGlobalBuffer("_SamplesPerPixel", SampleBuffer);

        int castShadow;
        if(enableShadow == true)
        {
            castShadow = 1;
        } else
        {
            castShadow = 0;
        }

        RT_MainShader.SetInt("_CastShadow", castShadow);
    }

    private void loadBuffer<T>(List<T> data, ref ComputeBuffer buffer, int size)
    where T : struct
    {
        if (buffer != null)
        {
            if (data.Count == 0 || buffer.count != data.Count || buffer.stride != size)
            {
                buffer.Release();
                buffer = null;
            }
        }

        // data set is not empty
        if (data.Count != 0)
        {
            if (buffer == null)
            {
                buffer = new ComputeBuffer(data.Count, size);
            }
            buffer.SetData(data);
        } 
    }
    private void SetGlobalBuffer(string name, ComputeBuffer buffer)
    {
        if (buffer != null)
        {
            RT_MainShader.SetBuffer(0, name, buffer);
        }
    }
    private void releaseBuffers()
    {
        releaseBuffer(MaterialBuffer);
        releaseBuffer(DirectionLightBuffer);
        releaseBuffer(PointLightBuffer);
        releaseBuffer(SampleBuffer);
        releaseBuffer(BVHTreeListBuffer);
        releaseBuffer(triangleListBuffer);
}

    private void releaseBuffer(ComputeBuffer buffer)
    {
            buffer.Release();
    }
}
