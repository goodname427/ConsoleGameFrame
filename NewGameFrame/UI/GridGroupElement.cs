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
        private readonly GridGroupElementSelectHelper _helper;

        public GridGroupElement()
        {
            _helper = new(this);
        }

        /// <summary>
        /// Grid布局
        /// </summary>
        public GridLayout Layout { get; set; } = GridLayout.Horizontal;
        /// <summary>
        /// 单边网格数量
        /// </summary>
        public int MaxGridCount { get; set; } = 1;
        /// <summary>
        /// 网格间距
        /// </summary>
        public Vector GridSpacing { get; set; } = new(0, -1);

        public CanvasElement? this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0)
                {
                    return null;
                }

                var index = Layout switch
                {
                    GridLayout.Vertical => x * MaxGridCount + y,
                    GridLayout.Horizontal => x + y * MaxGridCount,
                    _ => -1
                };

                if (index > -1 && index < ChildElements.Count)
                {
                    return ChildElements[index];
                }

                return null;
            }
        }

        public int GetGridColumnCount(int row)
        {
            return Layout switch
            {
                GridLayout.Vertical => GetMaxGridColumnCount() - (row >= GetGridRowCount(GetMaxGridColumnCount() - 1) ? 1 : 0),
                GridLayout.Horizontal => (row == GetMaxGridRowCount() - 1) ? (ChildCount % MaxGridCount == 0 ? MaxGridCount : (ChildCount % MaxGridCount)) : MaxGridCount,
                _ => 0
            };
        }
        public int GetMaxGridColumnCount()
        {
            return Layout switch
            {
                GridLayout.Vertical => ChildCount == 0 ? 0 : ((ChildCount - 1) / MaxGridCount + 1),
                GridLayout.Horizontal => Math.Min(ChildCount, MaxGridCount),
                _ => 0
            };
        }
        public int GetGridRowCount(int col)
        {
            return Layout switch
            {
                GridLayout.Vertical => (col == GetMaxGridColumnCount() - 1) ? (ChildCount % MaxGridCount == 0 ? MaxGridCount : (ChildCount % MaxGridCount)) : MaxGridCount,
                GridLayout.Horizontal => GetMaxGridRowCount() - (col >= GetGridColumnCount(GetMaxGridRowCount() - 1) ? 1 : 0),
                _ => 0
            };
        }
        public int GetMaxGridRowCount()
        {
            return Layout switch
            {
                GridLayout.Vertical => Math.Min(ChildCount, MaxGridCount),
                GridLayout.Horizontal => ChildCount == 0 ? 0 : ((ChildCount - 1) / MaxGridCount + 1),
                _ => 0
            };
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
                        x = i / MaxGridCount;
                        y = i % MaxGridCount;
                        break;
                    case GridLayout.Horizontal:
                        y = i / MaxGridCount;
                        x = i % MaxGridCount;
                        break;
                }

                ChildElements[i].Position = Position + new Vector(x * GridSpacing.X, y * GridSpacing.Y);
            }
        }
        public override void OnSelecting()
        {
            _helper.Update();
        }
    }
}
