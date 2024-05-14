using GameFrame.MathCore;
using System.Runtime.InteropServices;

namespace GameFrame.Render
{
    public class Screen
    {
        public static Screen? Instance { get; private set; }

        private ConsolePixel[,] _console = new ConsolePixel[0, 0];

        bool _isDrawFrame = true;
        /// <summary>
        /// 是否绘制屏幕边框
        /// </summary>
        public bool IsDrawFrame
        {
            get => _isDrawFrame;
            set
            {
                if (_isDrawFrame != value)
                {
                    DrawScreenFrame(!value);
                    _isDrawFrame = value;
                }
            }
        }

        /// <summary>
        /// 屏幕原点
        /// </summary>
        public Vector ScreenOrigin { get; set; } = new(1, 1);
        /// <summary>
        /// 光标停靠位置
        /// </summary>
        public Vector CursorHoldPosition { get; set; } = new Vector(0, Console.WindowHeight - 1);
        /// <summary>
        /// 提示信息
        /// </summary>
        public string HUD { get; set; } = "";

        public Screen()
        {
            Console.CursorVisible = false;

            if (IsDrawFrame)
            {
                DrawScreenFrame();
            }
            Draw(new Image());

            Instance = this;
        }

        /// <summary>
        /// 设置图像
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="image"></param>
        private void SetImage(int i, int j, ConsolePixel image, bool record = true)
        {
            var x = (ScreenOrigin.X + i) * 2;
            var y = ScreenOrigin.Y + j;

            if (x < 0 || y < 0 || x >= Console.WindowWidth || y >= Console.WindowHeight)
                return;

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = image.Color;
            Console.BackgroundColor = image.BackColor;
            Console.Write(image.Character is '\0' ? " " : image.Character);

            if (record)
                _console[i, j] = image;
        }

        private void DrawScreenFrame(bool clear = false)
        {
            var horizontal = clear ? ConsolePixel.Empty : '_';
            var vertical = clear ? '\0' : '|';

            var (width, height) = (_console.GetLength(0), _console.GetLength(1));
            for (int i = -1; i <= width; i++)
            {
                SetImage(i, -1, horizontal, false);
                SetImage(i, height, horizontal, false);
            }

            for (int j = -1; j <= height; j++)
            {
                SetImage(-1, j, vertical, false);
                SetImage(width, j, vertical, false);
            }
        }

        /// <summary>
        /// 更新屏幕
        /// </summary>
        /// <param name="renderCache"></param>
        public void Draw(Image renderCache)
        {
            if (Map.IsOutSide(_console, new(renderCache.Width - 1, renderCache.Height - 1)))
            {
                _console = Map.GetNewSizeMap(_console, renderCache.Width, renderCache.Height);

                if (IsDrawFrame)
                {
                    DrawScreenFrame();
                }
            }


            var (width, height) = (_console.GetLength(0), _console.GetLength(1));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (_console[i, j] != renderCache[i, j])
                        SetImage(i, j, renderCache[i, j]);
                }
            }

            Console.SetCursorPosition(ScreenOrigin.X + CursorHoldPosition.X, ScreenOrigin.Y + CursorHoldPosition.Y);
            Console.Write(HUD);
        }
    }
}
