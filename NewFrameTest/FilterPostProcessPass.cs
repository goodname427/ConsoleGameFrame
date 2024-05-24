using GameFrame.Core.Render;

namespace FrameTest
{
    internal class FilterPostProcessPass : IPostProcessPass
    {
        public void PostProcess(Image renderCache)
        {
            foreach (var (i, j) in renderCache.IndexEnumerator)
            {
                if (renderCache[i, j].Character == '@')
                {
                    renderCache[i, j] = '.';
                }
            }
        }
    }
}
