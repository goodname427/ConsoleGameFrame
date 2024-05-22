using GameFrame.Core.Render;
using GameFrame.Core;

namespace GameFrame.UI
{
    public class TextElement : CanvasElement
    {
        public string? Text { get; set; }
        
        public ConsoleColor Color { get; set; } = ConsoleColor.Black;

        public ConsoleColor BackColor { get; set; } = ConsoleColor.White;

        public override void PostProcess(Image renderCache)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            for (int i = 0; i < Text.Length; i++)
            {
                if (Map.IsOutSide(renderCache.Data, Position.X + i, Position.Y))
                {
                    break;
                }

                renderCache[Position.X + i, Position.Y] = new(Text[i], Color, BackColor);
            }
        }
    }
}
