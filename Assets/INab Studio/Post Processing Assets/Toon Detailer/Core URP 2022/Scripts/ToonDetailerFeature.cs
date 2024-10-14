using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace INab.Detailer.URP
{
    [Serializable]
    public class DetailerSettings
    {
        public enum DetailerType { Both = 0, Contours = 1, Cavity = 2 }
        public enum MaskUse { None = 0, NotEqual = 1, Equal = 2 }

        // General

        [SerializeField] public DetailerType _DetailerType = DetailerType.Both;
        [SerializeField] public MaskUse _MaskUse = MaskUse.None;
        [SerializeField] public LayerMask _MaskLayer;
        [SerializeField] public bool _FastMode = false;

        // Adjustments

        [SerializeField] public Color _ColorHue = Color.black;
        [SerializeField] public bool _UseFade = false;
        [SerializeField] public bool _FadeAffectsOnlyContours = false;
        [SerializeField] public float _FadeStart = 40;
        [SerializeField] public float _FadeEnd = 60;
        [SerializeField, Range(0, 1)] public float _BlackOffset = .2f;

        // Contours

        [SerializeField, Range(0, 1)] public float _ContoursIntensity = 1.0f;
        [SerializeField, Range(0, 3)] public float _ContoursThickness = 0.5f;
        [SerializeField, Range(0, 3)] public float _ContoursElevationStrength = 1;
        [SerializeField, Range(0, 0.9f)] public float _ContoursElevationSmoothness = 0;
        [SerializeField, Range(0, 3)] public float _ContoursDepressionStrength = 2;
        [SerializeField, Range(0, 0.9f)] public float _ContoursDepressionSmoothness = 0;

        // Cavity

        [SerializeField, Range(0, 1)] public float _CavityIntensity = 1.0f;
        [SerializeField, Range(0, 1)] public float _CavityRadius = 0.5f;
        [SerializeField, Range(0, 5)] public float _CavityStrength = 1.25f;
        [SerializeField, Range(1, 16)] public int _CavitySamples = 12;
    }

    public class ToonDetailerFeature : ScriptableRendererFeature
    {
        [SerializeField] private DetailerSettings m_Settings = new DetailerSettings();

        private Material m_Material;
        protected DetailerPass m_Pass = null;
        protected DepthMaskPass m_MaskPass = null;

        private const string k_UseContours = "_USE_CONTOURS";
        private const string k_UseCavity = "_USE_CAVITY";

        private const string k_Orthographic = "_ORTHOGRAPHIC";

        private const string k_FadeContoursOnly = "_FADE_COUNTOURS_ONLY";
        private const string k_FadeOn = "_FADE_ON";

        public virtual void CreatePass()
        {
            m_Pass = new DetailerPass();
            m_MaskPass = new DepthMaskPass(RenderPassEvent.AfterRenderingOpaques, m_Settings._MaskLayer, "DepthMask");
        }

        public override void Create()
        {
            if (m_Pass == null)
            {
                CreatePass();
            }

            GetMaterial();
            SetRenderPass();
        }

        public virtual void SetRenderPass()
        {
            m_Pass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if(m_Settings._MaskUse != DetailerSettings.MaskUse.None) renderer.EnqueuePass(m_MaskPass);

            m_Pass.Setup(m_Settings);
            if (m_Settings._MaskUse != DetailerSettings.MaskUse.None)
            {
                var sceneDepthMask = m_MaskPass.sceneDepthMask;
                m_Pass.SetupSceneDepthTarget(sceneDepthMask);
            }
            renderer.EnqueuePass(m_Pass);
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(m_Material);
        }

        private void GetMaterial()
        {
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/INab/Detailer"));

            m_Pass.m_Material = m_Material;
        }

        public class DetailerPass : ScriptableRenderPass
        {
            public Material m_Material;

            private DetailerSettings m_Settings;
            private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Toon Detailer");
            protected RenderTextureDescriptor m_Descriptor;

            private RTHandle destination;
            private RTHandle sceneDepthMaskHandle;

            internal DetailerPass()
            {
                m_Settings = new DetailerSettings();
            }

            public void SetupSceneDepthTarget(RTHandle sceneDepthMaskHandle)
            {
                this.sceneDepthMaskHandle = sceneDepthMaskHandle;
            }

            public void Setup(DetailerSettings featureSettings)
            {
                m_Settings = featureSettings;
                ConfigureInput(ScriptableRenderPassInput.Normal);
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                var desc = renderingData.cameraData.cameraTargetDescriptor;
                desc.depthBufferBits = 0; // Color and depth cannot be combined in RTHandles

                m_Descriptor = desc;

                var renderer = renderingData.cameraData.renderer;
                destination = renderer.cameraColorTargetHandle;

                #region materialProperties

                // General

                CoreUtils.SetKeyword(m_Material, k_Orthographic, renderingData.cameraData.camera.orthographic);

                switch (m_Settings._DetailerType)
                {
                    case DetailerSettings.DetailerType.Both:
                        m_Material.EnableKeyword(k_UseContours);
                        m_Material.EnableKeyword(k_UseCavity);
                        break;
                    case DetailerSettings.DetailerType.Contours:
                        m_Material.EnableKeyword(k_UseContours);
                        m_Material.DisableKeyword(k_UseCavity);
                        break;
                    case DetailerSettings.DetailerType.Cavity:
                        m_Material.DisableKeyword(k_UseContours);
                        m_Material.EnableKeyword(k_UseCavity);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Adjustments

                m_Material.SetColor("_ColorHue", m_Settings._ColorHue);
                m_Material.SetFloat("_FadeStart", m_Settings._FadeStart);
                m_Material.SetFloat("_FadeEnd", m_Settings._FadeEnd);
                m_Material.SetFloat("_BlackOffset", m_Settings._BlackOffset);

                if (m_Settings._UseFade)
                {
                    m_Material.EnableKeyword(k_FadeOn);
                }
                else
                {
                    m_Material.DisableKeyword(k_FadeOn);
                    m_Material.DisableKeyword(k_FadeContoursOnly);
                }


                if (m_Settings._FadeAffectsOnlyContours && m_Settings._UseFade)
                {
                    m_Material.EnableKeyword(k_FadeContoursOnly);
                    m_Material.DisableKeyword(k_FadeOn);
                }
                else
                {
                    m_Material.DisableKeyword(k_FadeContoursOnly);
                }

                // Countour

                m_Material.SetFloat("_ContoursIntensity", m_Settings._ContoursIntensity);
                m_Material.SetFloat("_ContoursThickness", m_Settings._ContoursThickness);
                m_Material.SetFloat("_ContoursElevationStrength", 3.0f * (m_Settings._ContoursElevationStrength * (0.7f / (1.0f - m_Settings._ContoursElevationSmoothness))));
                m_Material.SetFloat("_ContoursElevationSmoothness", 1 - m_Settings._ContoursElevationSmoothness);
                m_Material.SetFloat("_ContoursDepressionStrength", 2.0f * (m_Settings._ContoursDepressionStrength * (0.7f / (1.0f - m_Settings._ContoursDepressionSmoothness))));
                m_Material.SetFloat("_ContoursDepressionSmoothness", 1 - m_Settings._ContoursDepressionSmoothness);

                // Cavity 
                m_Material.SetFloat("_CavityIntensity", m_Settings._CavityIntensity);
                m_Material.SetFloat("_CavityRadius", m_Settings._CavityRadius);
                m_Material.SetFloat("_CavityStrength", m_Settings._CavityStrength);
                m_Material.SetInt("_CavitySamples", m_Settings._CavitySamples);

                #endregion
            }

            public virtual void BlitCommands(CommandBuffer cmd)
            {
                switch (m_Settings._MaskUse)
                {
                    case DetailerSettings.MaskUse.None:
                        Blitter.BlitCameraTexture(cmd, destination, destination, m_Material, 0);
                        break;
                    case DetailerSettings.MaskUse.NotEqual:
                        Blitter.BlitCameraTexture(cmd, destination, destination, m_Material, 1);
                        break;
                    case DetailerSettings.MaskUse.Equal:
                        Blitter.BlitCameraTexture(cmd, destination, destination, m_Material, 2);
                        break;
                }
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (renderingData.cameraData.cameraType == CameraType.Preview) return;

                if (m_Material == null)
                {
                    return;
                }

                CommandBuffer cmd = CommandBufferPool.Get("Toon Detailer");

                using (new ProfilingScope(cmd, m_ProfilingSampler))
                {
                    if (m_Settings._MaskUse != DetailerSettings.MaskUse.None) cmd.SetGlobalTexture("_DepthMaskRT", sceneDepthMaskHandle);

                    BlitCommands(cmd);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                destination = null;
            }
        }
    }
}
