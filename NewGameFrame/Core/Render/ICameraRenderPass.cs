namespace GameFrame.Core.Render
{
    public interface ICameraRenderPass
    {
        void RenderPass(Camera camera, Image renderCache);
    }
}
