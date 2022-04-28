using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class Main : MonoBehaviour
{
    public ComputeShader raymarching;
    public float debug;

    RenderTexture target;

    Camera cam;

    List<ComputeBuffer> trash = new List<ComputeBuffer>();

    public int Iterations = 1;
    
    struct Shape
    {
        int shapeType;
        Vector3 pos;
        Vector3 Scale;
        Vector3 Rotation;
        int operation;
        public static int GetSize()
        {
            return sizeof(float) * 9 + sizeof(int) * 2;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.current;
    }
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        cam = Camera.current;
        InitRenderTexture();

        setPrams();

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        raymarching.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);
        foreach  (ComputeBuffer buffer in trash)
        {
            buffer.Dispose();
        }
    }
    
    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight || true)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }

    void setPrams()
    {
        int kernelHandle = raymarching.FindKernel("CSMain");

        raymarching.SetTexture(kernelHandle, "Result", target);

        Shape[] ShapeData = new Shape[1];
        ComputeBuffer ShapeBuffer = new ComputeBuffer(ShapeData.Length, Shape.GetSize());
        raymarching.SetBuffer(kernelHandle, "shapes", ShapeBuffer);
        raymarching.SetInt("numShapes", 0);

        //raymarching.SetFloat("debug", debug);

        //Set iterations
        raymarching.SetInt("Iterations", Iterations);


        raymarching.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        raymarching.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);

        trash.Add(ShapeBuffer);
    }
}
