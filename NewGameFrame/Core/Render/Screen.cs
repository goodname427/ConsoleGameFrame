using System.Text;

namespace GameFrame.Core.Render
{
    /// <summary>
    /// 虚拟屏幕，会自适应相机输出的渲染缓冲区大小
    /// </summary>
    public class Screen : ISizeable
    {
        #region 控制台窗口尺寸
        /// <summary>
        /// 控制台窗口宽度，请使用ConsoleWindowWidth 代替 Console.WindowWidth
        /// </summary>
        public static int ConsoleWindowWidth { get; private set; }
        /// <summary>
        /// 控制台窗口高度，请使用ConsoleWindowHeight 代替 Console.WindowHeight
        /// </summary>
        public static int ConsoleWindowHeight { get; private set; }
        /// <summary>
        /// 更新控制台窗口尺寸
        /// </summary>
        public static void UpdateConsoleWindowSizeInfo()
        {
            ConsoleWindowHeight = Console.WindowHeight;
            ConsoleWindowWidth = Console.WindowWidth;
        }
        #endregion

        /// <summary>
        /// 主屏幕
        /// </summary>
        public static Screen? Main { get; private set; }

        #region 屏幕属性
        /// <summary>
        /// 自适应相机大小为屏幕大小
        /// </summary>
        public bool AutoAdjustConsoleWindow { get; set; } = true;
        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public int Width { get; private set; } = 21;
        /// <summary>
        /// 屏幕高度
        /// </summary>
        public int Height { get; private set; } = 21;

        /// <summary>
        /// 开启后会在宽度上拉伸
        /// </summary>
        public bool EnableDoubleWidth { get; set; } = true;

        private bool _isDrawFrame = false;
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
        public Vector ScreenOrigin { get; set; } = new(0, 0);
        /// <summary>
        /// 光标停靠位置
        /// </summary>
        public Vector CursorHoldPosition => new(0, Math.Clamp(Height - 2, 0, ConsoleWindowHeight - 2));
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

            Main ??= this;
        }

        /// <summary>
        /// 当前位置是否超出控制台窗口范围
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private static bool IsOutConsoleWindow(int left, int top)
        {
            return left < 0 || top < 0 || left >= ConsoleWindowWidth || top >= ConsoleWindowHeight;
        }
        /// <summary>
        /// 设置控制台窗口位置
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private static bool SetCursorPosition(int left, int top)
        {
            if (IsOutConsoleWindow(left, top))
                return false;

            try
            {
                Console.SetCursorPosition(left, top);
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 获取实际所需字符
        /// </summary>
        /// <param name="consolePixel"></param>
        /// <returns></returns>
        private static char GetCharacter(ConsolePixel consolePixel)
        {
            return consolePixel.IsEmpty() ? ' ' : consolePixel.Character;
        }

        /// <summary>
        /// 将渲染坐标转为实际控制台坐标
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private (int left, int top) TransformCoord(int i, int j)
        {
            var x = (ScreenOrigin.X + i) * (EnableDoubleWidth ? 2 : 1);
            var y = ScreenOrigin.Y + Height - j - 1;
            return (x, y);
        }

        /// <summary>
        /// 设置图像
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="consolePixel"></param>
        private void SetImage(int i, int j, ConsolePixel consolePixel)
        {
            var (left, top) = TransformCoord(i, j);

            if (!SetCursorPosition(left, top))
                return;

            Console.ForegroundColor = consolePixel.Color;
            Console.BackgroundColor = consolePixel.BackColor;
            Console.Write(GetCharacter(consolePixel));
        }

        /// <summary>
        /// 绘制屏幕边框
        /// </summary>
        /// <param name="clear"></param>
        private void DrawScreenFrame(bool clear = false)
        {
            var horizontal = clear ? ConsolePixel.Empty : '■';
            var vertical = clear ? ConsolePixel.Empty : '■';

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
            // 屏幕跟随窗口变化
            if (AutoAdjustConsoleWindow && (Width != ConsoleWindowWidth || Height != ConsoleWindowHeight))
            {
                Width = ConsoleWindowWidth;
                Height = ConsoleWindowHeight;

                Console.Clear();
                if (IsDrawFrame)
                {
                    DrawScreenFrame();
                }
                return;
            }
            
            var builder = new StringBuilder();
            var color = ConsoleColor.White;
            var backColor = ConsoleColor.Black;
            int left, top;
            ConsolePixel consolePixel;

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
                    left += builder.Length;
                    builder.Clear();
                }
            }

            // 绘制
            bool changeColor, changeBackColor, lastChangeColor = true, lastChangeBackColor = true;
            for (int j = 0; j < Height; j++)
            {
                // 跳转位置
                (left, top) = TransformCoord(0, j);
                if (!SetCursorPosition(left, top))
                    break;

                for (int i = 0; i < Width; i++)
                {
                    if (IsOutConsoleWindow(left + builder.Length + (EnableDoubleWidth ? 1 : 0), top))
                        break;

                    consolePixel = renderCache[i, j];
                    changeColor = (!consolePixel.IsEmpty() && color != consolePixel.Color);
                    changeBackColor = backColor != consolePixel.BackColor;

                    // 如果颜色改变则提前输出
                    if (changeColor || changeBackColor)
                    {
                        // 输出之前的缓存
                        DrawBuffer(lastChangeColor, lastChangeBackColor);
                        lastChangeColor = changeColor;
                        lastChangeBackColor = changeBackColor;

                        // 重新记录颜色
                        color = consolePixel.Color;
                        backColor = consolePixel.BackColor;
                    }

                    // 缓存字符
                    builder.Append(GetCharacter(consolePixel));

                    // 双倍宽度下追加字符
                    if (EnableDoubleWidth && builder[^1] < 256)
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
            if (SetCursorPosition(0, CursorHoldPosition.Y))
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(HUD);
            }
        }
    }
}
