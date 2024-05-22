using GameFrame.Core;
using GameFrame.Core.Render;
using System.Xml.Linq;

namespace GameFrame.UI
{
    

    public class Canvas : CanvasElement
    {  
        public override void PostProcess(Image renderCache)
        {
            foreach (var element in _elements)
            {
                element.Draw(renderCache);
            }
        }
    }
}
