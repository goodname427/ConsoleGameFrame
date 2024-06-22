namespace GameFrame.Core
{
    /// <summary>
    /// 监听控制台输入，并提供一定的入口
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// 键盘映射
        /// </summary>
        public static Dictionary<string, HashSet<ConsoleKey>> KeyMapping { get; private set; } = new()
        {
            { "Up", [ ConsoleKey.W, ConsoleKey.UpArrow ] },
            { "Left", [ ConsoleKey.A, ConsoleKey.LeftArrow ] },
            { "Down", [ ConsoleKey.S , ConsoleKey.DownArrow] },
            { "Right", [ ConsoleKey.D, ConsoleKey.RightArrow ] },
            { "Exit", [ ConsoleKey.Escape ] },
            { "Confirm", [ ConsoleKey.Enter ]},
        };

        /// <summary>
        /// 检测是否有任何键按下
        /// </summary>
        public static bool AnyKey => s_currentInputs.Count > 0;

        private static readonly List<ConsoleKey> s_currentInputs = [];
        /// <summary>
        /// 当前输入
        /// </summary>
        public static IEnumerable<ConsoleKey> CurrentInputs => s_currentInputs;

        /// <summary>
        /// 获取输入
        /// </summary>
        /// <returns></returns>
        public static void GetInput()
        {
            s_currentInputs.Clear();
            while (Console.KeyAvailable)
            {
                s_currentInputs.Add(Console.ReadKey().Key);
            }
        }
        /// <summary>
        /// 判断是否输入指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKey(ConsoleKey key)
        {
            return s_currentInputs.Contains(key);
        }
        /// <summary>
        /// 判断是否输入指定按钮
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetButton(string button)
        {
            if (!KeyMapping.TryGetValue(button, out var keys))
                return false;

            foreach (var key in keys)
            {
                if (GetKey(key))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取横轴向
        /// </summary>
        /// <returns></returns>
        public static int GetHorizontal()
        {
            return (GetButton("Left") ? -1 : 0) + (GetButton("Right") ? 1 : 0);
        }
        /// <summary>
        /// 获取纵轴向
        /// </summary>
        /// <returns></returns>
        public static int GetVertical()
        {
            return (GetButton("Down") ? -1 : 0) + (GetButton("Up") ? 1 : 0);
        }
        /// <summary>
        /// 获取方向
        /// </summary>
        /// <returns></returns>
        public static Vector GetDirection()
        {
            return new Vector(GetHorizontal(), GetVertical());
        }
    }
}
