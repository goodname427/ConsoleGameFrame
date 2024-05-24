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

        /// <summary>
        /// 父元素
        /// </summary>
        public CanvasElement? ParentElement { get; private set; } = null;

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
        /// <summary>
        /// 位置调整后调用
        /// </summary>
        protected virtual void OnPositionAdujusted() { }

        /// <summary>
        /// 子元素
        /// </summary>
        protected List<CanvasElement> ChildElements = [];
        /// <summary>
        /// 添加Element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual CanvasElement AddElement(CanvasElement element)
        {
            CanvasElement? parent = this;
            while (parent != null)
            {
                if (parent == element)
                {
                    return this;
                }

                parent = parent.ParentElement;
            }

            RemoveElement(element);

            element.ParentElement = this;
            ChildElements.Add(element);
            return this;
        }
        /// <summary>
        /// 移除Element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual CanvasElement RemoveElement(CanvasElement element)
        {
            if (element.ParentElement != this)
            {
                return this;
            }

            element.ParentElement = null;
            ChildElements.Remove(element);
            return this;
        }


        /// <summary>
        /// 绘制当前元素以及子元素
        /// </summary>
        /// <param name="renderCache"></param>
        /// <param name="parentPostion"></param>
        public virtual void Draw(Image renderCache, Vector parentPostion)
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
                } + parentPostion;
            }

            OnPositionAdujusted();

            PostProcess(renderCache);

            foreach (var element in ChildElements)
            {
                element.Draw(renderCache, Position);
            }
        }
        /// <summary>
        /// 处理自身的绘制
        /// </summary>
        /// <param name="renderCache"></param>
        public abstract void PostProcess(Image renderCache);


        #region UI交互
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// 选中元素
        /// </summary>
        public virtual void Select()
        {
            if (IsSelected)
            {
                return;
            }

            // 先选中所有祖先元素
            var acescents = new Stack<CanvasElement>();
            CanvasElement? parent = ParentElement;
            while (parent != null && !parent.IsSelected)
            {
                acescents.Push(parent);
                parent = parent?.ParentElement;
            }

            while (acescents.Count > 0)
            {
                acescents.Pop().Select();

            }

            IsSelected = true;
            OnSelected();
        }
        /// <summary>
        /// 取消选中元素
        /// </summary>
        public virtual void Unselect()
        {
            if (!IsSelected)
            {
                return;
            }

            IsSelected = false;
            OnUnselected();
            
            // 取消选中所有子元素
            foreach (var child in ChildElements)
            {
                child.Unselect();
            }
        }
        /// <summary>
        /// 元素被选中时调用
        /// </summary>
        protected virtual void OnSelected() { }
        /// <summary>
        /// 元素在选中状态下会一直调用
        /// </summary>
        protected virtual void OnSelecting() { }
        /// <summary>
        /// 元素被取消选中时调用
        /// </summary>
        protected virtual void OnUnselected() { }
        /// <summary>
        /// 元素确认时调用
        /// </summary>
        protected virtual void OnConfirm() { }
        #endregion
    }
}
