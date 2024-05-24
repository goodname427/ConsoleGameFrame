using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    public class Canvas : CanvasElement
    {
        public override void Draw(Image renderCache, Vector parentPostion)
        {
            PostProcess(renderCache);
        }

        public override void PostProcess(Image renderCache)
        {
            foreach (var element in ChildElements)
            {
                element.Draw(renderCache, Vector.Zero);
            }
        }
    }
}
