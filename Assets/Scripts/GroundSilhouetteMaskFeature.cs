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
        private RenderStateBlock stateBlock;

        public GroundSilhouetteMaskPass(LayerMask layerMask)
        {
            this.layerMask = layerMask;

            StencilState stencilState = new StencilState(
                enabled: true,
                readMask: 0xFF,
                writeMask: 0xFF,
                compareFunction: CompareFunction.Always,
                passOperation: StencilOp.Replace,
                failOperation: StencilOp.Keep,
                zFailOperation: StencilOp.Keep
            );

            RenderTargetBlendState renderTargetBlend =
                new RenderTargetBlendState(
                    (ColorWriteMask)0,
                    BlendMode.One,
                    BlendMode.Zero,
                    BlendMode.One,
                    BlendMode.Zero,
                    BlendOp.Add,
                    BlendOp.Add
                );

            BlendState blendState = new BlendState
            {
                blendState0 = renderTargetBlend
            };

            stateBlock = new RenderStateBlock(
                RenderStateMask.Blend |
                RenderStateMask.Stencil
            );

            stateBlock.blendState = blendState;
            stateBlock.stencilState = stencilState;
            stateBlock.stencilReference = 1;
        }

        public override void Execute(
            ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            DrawingSettings drawing =
                CreateDrawingSettings(
                    new ShaderTagId("UniversalForward"),
                    ref renderingData,
                    SortingCriteria.CommonOpaque
                );

            drawing.SetShaderPassName(
                1,
                new ShaderTagId("UniversalForwardOnly")
            );

            drawing.SetShaderPassName(
                2,
                new ShaderTagId("SRPDefaultUnlit")
            );

            FilteringSettings filtering =
                new FilteringSettings(
                    RenderQueueRange.opaque,
                    layerMask
                );

            context.DrawRenderers(
                renderingData.cullResults,
                ref drawing,
                ref filtering,
                ref stateBlock
            );
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    protected override void Dispose(bool disposing)
    {
    }
}