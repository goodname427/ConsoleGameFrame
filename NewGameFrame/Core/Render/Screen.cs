using System.Linq.Expressions;
using System.Text;

namespace GameFrame.Core.Render
{
    /// <summary>
    /// 虚拟屏幕，会自适应相机输出的渲染缓冲区大小
    /// </summary>
    public class Screen
    {
        public static int ConsoleWindowWidth { get; private set; }
        public static int ConsoleWindowHeight { get; private set; }
        public static void UpdateConsoleWindowSizeInfo()
        {
            ConsoleWindowHeight = Console.WindowHeight;
            ConsoleWindowWidth = Console.WindowWidth;
        }

        public static Screen? Instance { get; private set; }

        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 屏幕高度
        /// </summary>
        public int Height { get; private set; }

        #region 屏幕属性
        /// <summary>
        /// 开启后会在宽度上拉伸
        /// </summary>
        public bool EnableDoubleWidth { get; set; } = true;
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
        public Vector CursorHoldPosition => new(0, ConsoleWindowHeight - 2);
        #endregion

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


        private static bool IsOutConsoleWindow(int x, int y)
        {
            return x < 0 || y < 0 || x >= ConsoleWindowWidth || y >= ConsoleWindowHeight;
        }

        private static bool SetCursorPosition(int x, int y)
        {
            if (IsOutConsoleWindow(x, y))
                return false;

            Console.SetCursorPosition(x, y);
            return true;
        }
        private (int x, int y) TransformCoord(int i, int j)
        {
            var x = (ScreenOrigin.X + i) * (EnableDoubleWidth ? 2 : 1);
            var y = ScreenOrigin.Y + j;
            return (x, y);
        }

        /// <summary>
        /// 设置图像
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="image"></param>
        private void SetImage(int i, int j, ConsolePixel image)
        {
            var (x, y) = TransformCoord(i, j);

            if (!SetCursorPosition(x, y))
                return;

            Console.ForegroundColor = image.Color;
            Console.BackgroundColor = image.BackColor;
            Console.Write(image.Character is '\0' ? " " : image.Character);
        }

        /// <summary>
        /// 绘制屏幕边框
        /// </summary>
        /// <param name="clear"></param>
        private void DrawScreenFrame(bool clear = false)
        {
            var horizontal = clear ? ConsolePixel.Empty : '〓';
            var vertical = clear ? '\0' : '〓';

            for (int i = -1; i <= Width; i++)
            {
                SetImage(i, -1, horizontal);
                SetImage(Width, Height, horizontal);
            }

            for (int j = -1; j <= Height; j++)
            {
                SetImage(-1, j, vertical);
                SetImage(Width, j, vertical);
            }
        }

        /// <summary>
        /// 更新屏幕
        /// </summary>
        /// <param name="renderCache"></param>
        public void Draw(Image renderCache)
        {
            // 重置屏幕
            if (Width != renderCache.Width || Height != renderCache.Height)
            {
                Width = renderCache.Width;
                Height = renderCache.Height;

                if (IsDrawFrame)
                {
                    DrawScreenFrame();
                }
            }

            var builder = new StringBuilder();
            var color = ConsoleColor.White;
            var backColor = ConsoleColor.Black;
            int x = 0, y = 0;
            ConsolePixel pixel;

            void DrawBuffer(bool changeColor, bool changeBackColor)
            {
                if (changeColor)
                {
                    Console.ForegroundColor = color;
                }
                if (changeBackColor)
                {
                    Console.BackgroundColor = backColor;
                }

                if (builder.Length > 0)
                {   
                    Console.Write(builder.ToString());
                    x += builder.Length;
                    builder.Clear();
                }
            }

            // 绘制
            bool changeColor, changeBackColor, lastChangeColor = true, lastChangeBackColor = true;
            for (int j = 0; j < Height; j++)
            {
                // 跳转位置
                (x, y) = TransformCoord(0, j);
                if (!SetCursorPosition(x, y))
                    break;

                for (int i = 0; i < Width; i++)
                {
                    if (IsOutConsoleWindow(x + builder.Length + (EnableDoubleWidth ? 1 : 0), y))
                        break;

                    pixel = renderCache[i, j];
                    changeColor = (!pixel.IsEmpty() && color != pixel.Color);
                    changeBackColor = backColor != pixel.BackColor;

                    // 如果颜色改变则提前输出
                    if (changeColor || changeBackColor)
                    {
                        // 输出之前的缓存
                        DrawBuffer(lastChangeColor, lastChangeBackColor);
                        lastChangeColor = changeColor;
                        lastChangeBackColor = changeBackColor;

                        // 重新记录颜色
                        color = pixel.Color;
                        backColor = pixel.BackColor;
                    }

                    // 缓存字符
                    if (pixel.IsEmpty())
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Append(pixel.Character);
                    }

                    if (EnableDoubleWidth)
                    {
                        builder.Append(' ');
                    }
                }

                // 输出
                DrawBuffer(lastChangeColor, lastChangeBackColor);
                lastChangeColor = false;
                lastChangeBackColor = false;
            }

            // 绘制HUD
            SetCursorPosition(Math.Clamp(CursorHoldPosition.X, 0, ConsoleWindowWidth - 1), Math.Clamp(CursorHoldPosition.Y, 0, ConsoleWindowHeight - 1));
            Console.WriteLine(HUD);
        }
    }
}
