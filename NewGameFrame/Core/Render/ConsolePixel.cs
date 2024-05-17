namespace GameFrame.Core.Render
{
    public struct ConsolePixel
    {
        public static readonly ConsolePixel Empty = new();

        #region 运算符
        public static implicit operator ConsolePixel(char character)
        {
            return new() { Character = character };
        }
        public static bool operator ==(ConsolePixel left, ConsolePixel right)
        {
            return left.Character == right.Character && left.Color == right.Color;
        }
        public static bool operator !=(ConsolePixel left, ConsolePixel right)
        {
            return !(left == right);
        }
        #endregion

        #region 基础属性
        public char Character { get; set; } = '\0';
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
        #endregion

        #region 构造函数
        public ConsolePixel()
        {
        }
        public ConsolePixel(char character, ConsoleColor color = ConsoleColor.White, ConsoleColor backColor = ConsoleColor.Black)
        {
            Character = character;
            Color = color;
            BackColor = backColor;
        }
        #endregion

        public readonly bool IsEmpty()
        {
            return Character == '\0';
        }
        public override readonly bool Equals(object? obj)
        {
            return obj is not null && obj is ConsolePixel pixel && this == pixel;
        }
        public override readonly int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
