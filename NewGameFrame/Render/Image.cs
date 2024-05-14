using GameFrame.MathCore;

namespace GameFrame.Render
{
    public class Image
    {
        /// <summary>
        /// 图片数据
        /// </summary>
        public ConsolePixel[,] Data { get; private set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get => Size.X; set => Size = Size with { X = value }; }
        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get => Size.Y; set => Size = Size with { Y = value }; }
        /// <summary>
        /// 图片尺寸
        /// </summary>
        public Vector Size { get; set; } = Vector.Zero;

        /// <summary>
        /// 图片锚点，默认为中心点
        /// </summary>
        public Vector Pivot { get; set; }

        /// <summary>
        /// 遍历图片
        /// </summary>
        public IEnumerable<(ConsolePixel pixel, int i, int j)> Enumerator
        {
            get
            {
                for (int i =0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        yield return (Data[i, j], i, j);
                    }
                }
            }
        }

        public ConsolePixel this[int x, int y]
        {
            get
            {
                return Data[x, y];
            }
            set
            {
                Data[x, y] = value;
            }
        }

        public ConsolePixel this[Vector position]
        {
            get
            {
                return this[position.X, position.Y];
            }
            set
            {
                Data[position.X, position.Y] = value;
            }
        }

        public Image() : this(0, 0) { }

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            Pivot = new Vector(Width / 2, Height / 2);
            Data = new ConsolePixel[Width, Height];
        }

        public Image(ConsolePixel[,] data)
        {
            Width = data.GetLength(0);
            Height = data.GetLength(1);
            Pivot = new Vector(Width / 2, Height / 2);
            Data = data;
        }

        public Image(ConsolePixel pixel) : this(1, 1)
        {
            Data[0, 0] = pixel;
        }

        public Image(ConsolePixel pixel, int width, int height) : this(width, height)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Data[x, y] = pixel;
                }
            }
        }

        public void Resize(int newWidth, int newHeight)
        {
            Data = Map.GetNewSizeMap(Data, newWidth, newHeight);
            Width = newWidth;
            Height = newHeight;
        }
    }
}
