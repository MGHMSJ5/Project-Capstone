using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GroundSilhouetteMaskFeature : ScriptableRendererFeature
{
    [SerializeField]
    private LayerMask groundLayer;

    private GroundSilhouetteMaskPass maskPass;

    public override void Create()
    {
        maskPass = new GroundSilhouetteMaskPass(groundLayer);

        // Ground mask must be rendered after opaque geometry
        // but before the player silhouette.
        maskPass.renderPassEvent =
            RenderPassEvent.AfterRenderingOpaques;
    }

    public override void AddRenderPasses(
        ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        renderer.EnqueuePass(maskPass);
    }

    private class GroundSilhouetteMaskPass : ScriptableRenderPass
    {
        private readonly LayerMask layerMask;

        private Material maskMaterial;

        public GroundSilhouetteMaskPass(
            LayerMask layerMask)
        {
            this.layerMask = layerMask;

            Shader shader =
                Shader.Find("Hidden/GroundSilhouetteMask");

            if (shader != null)
            {
                maskMaterial =
                    new Material(shader);
            }
        }

        public override void Execute(
            ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            if (maskMaterial == null)
                return;

            Camera camera =
                renderingData.cameraData.camera;

            if (camera == null)
                return;

            SortingCriteria sorting =
                SortingCriteria.CommonOpaque;

            DrawingSettings drawing =
                CreateDrawingSettings(
                    new ShaderTagId("UniversalForward"),
                    ref renderingData,
                    sorting
                );

            // Also catch objects using other common URP passes.
            drawing.SetShaderPassName(
                1,
                new ShaderTagId("UniversalForwardOnly")
            );

            drawing.overrideMaterial =
                maskMaterial;

            FilteringSettings filtering =
                new FilteringSettings(
                    RenderQueueRange.opaque,
                    layerMask
                );

            context.DrawRenderers(
                renderingData.cullResults,
                ref drawing,
                ref filtering
            );
        }

        public override void OnCameraCleanup(
            CommandBuffer cmd)
        {
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (maskPass != null)
        {
            // Material is intentionally cleaned up by Unity
            // when the renderer feature is destroyed.
        }
    }
}