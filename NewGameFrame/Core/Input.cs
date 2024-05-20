using System.Collections.ObjectModel;

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
        public static ReadOnlyDictionary<string, ConsoleKey> KeyMapping { get; private set; } = new(new Dictionary<string, ConsoleKey>
        {
            {"Up", ConsoleKey.W },
            {"Left", ConsoleKey.A },
            {"Down", ConsoleKey.S },
            {"Right", ConsoleKey.D },
        });

        /// <summary>
        /// 检测是否有任何键按下
        /// </summary>
        public static bool AnyKey => _currentInputs.Count > 0;

        private static readonly List<ConsoleKey> _currentInputs = [];
        /// <summary>
        /// 当前输入
        /// </summary>
        public static ReadOnlyCollection<ConsoleKey> CurrentInputs => _currentInputs.AsReadOnly();

        /// <summary>
        /// 获取输入
        /// </summary>
        /// <returns></returns>
        public static void GetInput()
        {
            _currentInputs.Clear();
            while (Console.KeyAvailable)
            {
                _currentInputs.Add(Console.ReadKey().Key);
            }
        }
        /// <summary>
        /// 判断是否输入指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKey(ConsoleKey key)
        {
            return _currentInputs.Contains(key);
        }
        /// <summary>
        /// 获取横轴向
        /// </summary>
        /// <returns></returns>
        public static int GetHorizontal()
        {
            return (GetKey(KeyMapping["Left"]) ? -1 : 0) + (GetKey(KeyMapping["Right"]) ? 1 : 0);
        }
        /// <summary>
        /// 获取纵轴向
        /// </summary>
        /// <returns></returns>
        public static int GetVertical()
        {
            return (GetKey(KeyMapping["Down"]) ? -1 : 0) + (GetKey(KeyMapping["Up"]) ? 1 : 0);
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
