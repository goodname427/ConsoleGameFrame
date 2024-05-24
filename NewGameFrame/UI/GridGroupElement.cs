using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    public enum GridLayout
    {
        Vertical,
        Horizontal
    }

    public class GridGroupElement : CanvasElement
    {
        public GridLayout Layout { get; set; } = GridLayout.Horizontal;
        public int GridCount { get; set; } = 1;
        public Vector GridSpacing { get; set; } = new(0, -1);

        public CanvasElement? this[int x, int y]
        {
            get
            {
                var index = Layout switch
                {
                    GridLayout.Vertical => x * GridCount + y,
                    GridLayout.Horizontal => x + y * GridCount,
                    _ => -1
                };

                if (index > -1 && index < ChildElements.Count)
                {
                    return ChildElements[index];
                }

                return null;
            }
        }

        public override CanvasElement AddElement(CanvasElement element)
        {
            base.AddElement(element);

            element.Align = CanvasElementAlign.Custom;

            return this;
        }

        public override void PostProcess(Image renderCache)
        {

        }

        protected override void OnPositionAdujusted()
        {
            // 调整子Element的节点
            for (int i = 0; i < ChildElements.Count; i++)
            {
                int x = 0, y = 0;
                switch (Layout)
                {
                    case GridLayout.Vertical:
                        x = i / GridCount;
                        y = i % GridCount;
                        break;
                    case GridLayout.Horizontal:
                        y = i / GridCount;
                        x = i % GridCount;
                        break;
                }

                ChildElements[i].Position = Position + new Vector(x * GridSpacing.X, y * GridSpacing.Y);
            }
        }
    }
}
