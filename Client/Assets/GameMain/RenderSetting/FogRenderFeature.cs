using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FogRenderFeature : ScriptableRendererFeature
{
    class FogPass : ScriptableRenderPass
    {
        ProfilingSampler m_ProfilingSampler = new ProfilingSampler("FogBlit");
        Material m_Material;
        RTHandle m_CameraColorTarget;

        Color m_FogColor;

        //float m_FogDensity;
        float m_FogStart;
        float m_FogEnd;
        float m_NoiseCellSize;
        float m_NoiseRoughness;
        float m_NoisePersistance;
        Vector3 m_NoiseSpeed;
        float m_NoiseScale;

        public FogPass(Material material)
        {
            m_Material = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void SetTarget(RTHandle colorHandle, Color fogColor, float noiseCellSize, float noiseRoughness, float noisePersistance, Vector3 noiseSpeed, float noiseScale, float fogStart, float fogEnd)
        {
            m_CameraColorTarget = colorHandle;
            m_FogStart = fogStart;
            m_FogEnd = fogEnd;
            m_FogColor = fogColor;
            m_NoiseCellSize = noiseCellSize;
            m_NoiseRoughness = noiseRoughness;
            m_NoisePersistance = noisePersistance;
            m_NoiseSpeed = noiseSpeed;
            m_NoiseScale = noiseScale;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureTarget(m_CameraColorTarget);
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cameraData = renderingData.cameraData;
            if (cameraData.camera.cameraType != CameraType.Game)
                return;

            if (m_Material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
                m_Material.SetColor("_FogColor", m_FogColor);
                m_Material.SetFloat("_FogStart", m_FogStart);
                m_Material.SetFloat("_FogEnd", m_FogEnd);
                m_Material.SetFloat("_NoiseCellSize", m_NoiseCellSize);
                m_Material.SetFloat("_NoiseRoughness", m_NoiseRoughness);
                m_Material.SetFloat("_NoisePersistance", m_NoisePersistance);
                m_Material.SetVector("_NoiseSpeed", m_NoiseSpeed);
                m_Material.SetFloat("_NoiseScale", m_NoiseScale);

                m_Material.SetMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
                Blitter.BlitCameraTexture(cmd, m_CameraColorTarget, m_CameraColorTarget, m_Material, 0);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }
    }

    public Shader m_shader;
    public Color m_FogColor;
    public float m_FogStart;
    public float m_FogEnd;
    public float m_NoiseCellSize;
    public float m_NoiseRoughness;
    public float m_NoisePersistance;
    public Vector3 m_NoiseSpeed;
    public float m_NoiseScale;

    Material m_Material;

    FogPass m_RenderPass = null;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(m_RenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_RenderPass.ConfigureInput(ScriptableRenderPassInput.Color);

            m_RenderPass.SetTarget(renderer.cameraColorTargetHandle, m_FogColor, m_NoiseCellSize, m_NoiseRoughness, m_NoisePersistance, m_NoiseSpeed, m_NoiseScale, m_FogStart, m_FogEnd);
        }
    }


    public override void Create()
    {
        if (m_shader != null)
            m_Material = new Material(m_shader);

        m_RenderPass = new FogPass(m_Material);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_Material);
    }
}