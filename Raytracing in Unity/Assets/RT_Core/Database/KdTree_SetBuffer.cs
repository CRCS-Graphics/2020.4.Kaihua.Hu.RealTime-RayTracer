using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RT_Core
{
    private ComputeBuffer BVHTreeListBuffer;
    private ComputeBuffer triangleListBuffer;

    private void initBVHTreeBuffers()
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


        loadBuffer(BVHTreeList, ref BVHTreeListBuffer, sizeof(float) * 6 + sizeof(uint) * 4);
        SetGlobalBuffer("_BVHTreeList", BVHTreeListBuffer);

        loadBuffer(triangleList, ref triangleListBuffer, sizeof(float) * 12 + sizeof(uint));
        SetGlobalBuffer("_TriangleList", triangleListBuffer);
    }
}