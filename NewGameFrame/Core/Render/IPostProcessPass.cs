namespace GameFrame.Core.Render
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostProcessPass
    {
        void PostProcess(Image renderCache);
    }
}
