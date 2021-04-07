using System;
using UnityEngine;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering.Universal;

namespace Shaders.Render_Pipeline.Blit
{
  public class Blit : ScriptableRendererFeature
  {
    public enum Target
    {
      Color,
      Texture
    }

    public BlitSettings settings = new BlitSettings();

    private BlitPass blitPass;
    private RenderTargetHandle m_RenderTextureHandle;

    public override void Create()
    {
      var passIndex = settings.blitMaterial != null ? settings.blitMaterial.passCount - 1 : 1;
      settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
      blitPass = new BlitPass(settings.Event, settings.blitMaterial, settings.blitMaterialPassIndex, name);
      m_RenderTextureHandle.Init(settings.textureId);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
      var src = renderer.cameraColorTarget;
      var dest = settings.destination == Target.Color ? RenderTargetHandle.CameraTarget : m_RenderTextureHandle;

      if (settings.blitMaterial == null)
      {
        Debug.LogWarningFormat(
          "Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.",
          GetType().Name);
        return;
      }

      blitPass.Setup(src, dest);
      renderer.EnqueuePass(blitPass);
    }

    [Serializable]
    public class BlitSettings
    {
      public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

      public Material blitMaterial;
      public int blitMaterialPassIndex = -1;
      public Target destination = Target.Color;
      public string textureId = "_BlitPassTexture";
    }
  }
}