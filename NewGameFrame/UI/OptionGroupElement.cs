using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    public class OptionGroupElement : CanvasElement
    {
        public class Option(string text, Action<Option>? confirmed)
        {
            public string? Text { get; private set; } = text;
            public Action<Option>? Confirmed { get; private set; } = confirmed;
        }

        private readonly TextElement _titleElement;
        public string? Title { get => _titleElement.Text; set => _titleElement.Text = value; }

        public OptionGroupElement(string? title = "Edit Me")
        {
            base.AddElement(_titleElement = new TextElement { Text = title, Align = CanvasElementAlign.BottomLeft, Color = ConsoleColor.White, BackColor = ConsoleColor.Black });
            base.AddElement(_grid = new GridGroupElement { Align = CanvasElementAlign.BottomLeft, PositionOffset = new(0, -1) });
        }

        private readonly GridGroupElement _grid;
        private readonly Dictionary<Option, TextElement> _optionElements = [];
        /// <summary>
        /// 遍历所有选项
        /// </summary>
        public IEnumerable<Option> Options
        {
            get
            {
                foreach (var p in _optionElements)
                {
                    yield return p.Key;
                }
            }
        }
        /// <summary>
        /// 添加新选项
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public OptionGroupElement AddOption(Option option)
        {
            var textElement = new TextElement { Text = option.Text, Color = ConsoleColor.White, BackColor = ConsoleColor.Black };
            textElement.Confirmed += () => option.Confirmed?.Invoke(option);
            _grid.AddElement(textElement);
            _optionElements.Add(option, textElement);
            return this;
        }
        /// <summary>
        /// 移除选项
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public OptionGroupElement RemoveOption(Option option)
        {
            if (_optionElements.TryGetValue(option, out var element))
            {
                _grid.RemoveElement(element);
            }

            return this;
        }
        
        /// <summary>
        /// 开始选择
        /// </summary>
        public void Choose()
        {
            _grid.Select();
        }
        /// <summary>
        /// 结束选择
        /// </summary>
        public void Unchoose()
        {
            _grid?.Unselect();
        }

        /// <summary>
        /// 请使用<code>AddOption</code>取代该函数
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override CanvasElement AddElement(CanvasElement element)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 请使用<code>AddOption</code>取代该函数
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override CanvasElement RemoveElement(CanvasElement element)
        {
            throw new NotImplementedException();
        }

        public override void PostProcess(Image renderCache)
        {

        }
    }
}
