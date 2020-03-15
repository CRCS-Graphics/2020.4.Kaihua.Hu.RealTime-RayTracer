using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RT_Core : MonoBehaviour
{
    public ComputeShader RT_MainShader;
    public Texture SkyboxTexture;

    private RenderTexture RT_Result;
    private Camera MainCamera;


    private void Awake()
    {
        MainCamera = GetComponent<Camera>();
        initBVHTreeDataBase();
        initBVHTreeBuffers();
        Debug.Log("triangle: " + triangleList.Count);
    }

    private void OnApplicationQuit()
    {
        releaseBuffers();
    }
    private void OnRenderImage(RenderTexture input, RenderTexture output)
    {
        if(lasyUpdateScene)
        {
            LasyUpdateBVHTreeDataBase();
        } else
        {
            initBVHTreeDataBase();
        }

        initBVHTreeBuffers();

        Render(output);
    }

    private void Render(RenderTexture screen)
    {
        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        RT_MainShader.SetTexture(0, "Result", RT_Result);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RT_MainShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        // Blit the result texture to the screen
        Graphics.Blit(RT_Result, screen);
        //RTTriangle [] RES = new RTTriangle[triangleList.Count];
        //triangleListBuffer.GetData(RES);
    }

    /// <summary>
    /// This method ensure our RenderTexture is initialized properly for ComputeShader
    /// The code snipet is copied from http://blog.three-eyed-games.com/2018/05/03/gpu-ray-tracing-in-unity-part-1/
    /// Reference: http://blog.three-eyed-games.com/2018/05/03/gpu-ray-tracing-in-unity-part-1/
    /// </summary>
    private void InitRenderTexture()
    {
        if (RT_Result == null || RT_Result.width != Screen.width || RT_Result.height != Screen.height)
        {
            // Release render texture if we already have one
            if (RT_Result != null)
                RT_Result.Release();
            // Get a render target for Ray Tracing
            RT_Result = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            RT_Result.enableRandomWrite = true;
            RT_Result.Create();
        }
    }
}
