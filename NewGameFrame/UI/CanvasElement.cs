using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    public enum CanvasElementAlign
    {
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center,
        Custom
    }

    public abstract class CanvasElement : IPostProcessPass
    {
        /// <summary>
        /// UI的对齐方式
        /// </summary>
        public CanvasElementAlign Align { get; set; } = CanvasElementAlign.TopLeft;
        private Vector _position;
        /// <summary>
        /// UI的位置
        /// </summary>
        public Vector Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (Align == CanvasElementAlign.Custom)
                {
                    _position = value;
                }
            }
        }

        protected readonly List<CanvasElement> _elements = [];
        public void AddElement(CanvasElement element)
        {
            _elements.Add(element);
        }
        public void RemoveElement(CanvasElement element)
        {
            _elements.Remove(element);
        }

        public void Draw(Image renderCache)
        {
            if (Align != CanvasElementAlign.Custom)
            {
                _position = Align switch
                {
                    CanvasElementAlign.Left => new(0, renderCache.Height / 2),
                    CanvasElementAlign.TopLeft => new(0, renderCache.Height - 1),
                    CanvasElementAlign.BottomLeft => new(0, 0),
                    CanvasElementAlign.Right => new(renderCache.Width - 1, renderCache.Height / 2),
                    CanvasElementAlign.TopRight => new(renderCache.Width - 1, renderCache.Height - 1),
                    CanvasElementAlign.BottomRight => new(renderCache.Width - 1, 0),
                    CanvasElementAlign.Top => new(renderCache.Width / 2, renderCache.Height - 1),
                    CanvasElementAlign.Bottom => new(renderCache.Width / 2, 0),
                    CanvasElementAlign.Center => new(renderCache.Width / 2, renderCache.Height / 2),
                    _ => new()
                };
            }

            PostProcess(renderCache);

            foreach (var element in _elements)
            {
                element.Draw(renderCache);
            }
        }

        public abstract void PostProcess(Image renderCache);
    }
}
