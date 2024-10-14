using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace INab.Detailer.URP
{
    public class DepthMaskPass : ScriptableRenderPass
    {
        private readonly string profilingName;

        private readonly Material maskMaterial;
        private readonly List<ShaderTagId> shaderTagIdList;

        private FilteringSettings filteringSettings;

        public RTHandle sceneDepthMask;

        public DepthMaskPass(RenderPassEvent renderPassEvent, int layerMask, string profilingName)
        {
            this.profilingName = profilingName;
            this.renderPassEvent = renderPassEvent;

            filteringSettings = new FilteringSettings(RenderQueueRange.all, layerMask);

            maskMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/INab/ToonDetailer/Mask"));

            shaderTagIdList = new List<ShaderTagId>()
            {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefoultUnlit"),
                new ShaderTagId("DepthOnly"),
                new ShaderTagId("UniversalGBuffer"),
                new ShaderTagId("DepthNormalsOnly"),
                new ShaderTagId("Universal2D"),
                new ShaderTagId("SRPDefaultUnlit"),
            };

            sceneDepthMask = RTHandles.Alloc("_DepthMaskRT", name: "_DepthMaskRT");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor textureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            textureDescriptor.colorFormat = RenderTextureFormat.RFloat;
            textureDescriptor.msaaSamples = 1;

            cmd.GetTemporaryRT(Shader.PropertyToID(sceneDepthMask.name), textureDescriptor, FilterMode.Point);

            ConfigureTarget(sceneDepthMask);
            ConfigureClear(ClearFlag.All, Color.black);
        }



        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Preview) return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler(
                profilingName)))
            {
                DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawingSettings.overrideMaterial = maskMaterial;
                drawingSettings.enableDynamicBatching = true;

                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(sceneDepthMask.name));
        }
    }
}