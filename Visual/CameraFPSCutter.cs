using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFPSCutter : MonoBehaviour
{
    public int FPS = 10;
    public RenderTexture renderTexture;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (renderTexture == null)
            renderTexture = Instantiate(source) as RenderTexture;

        if (Time.frameCount % FPS == 0)
            Graphics.Blit(source, renderTexture);
        Graphics.Blit(renderTexture, destination);
    }
}
