using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RTPipeline : RenderPipeline 
{
    private ComputeShader m_mainShader;

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        context.DrawSkybox(cameras[0]);
        context.Submit();
    }
}