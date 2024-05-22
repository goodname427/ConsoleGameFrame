using System.Net;

namespace GameFrame.Core.Render
{
    public class Image
    {
        /// <summary>
        /// 图片数据
        /// </summary>
        public ConsolePixel[,] Data { get; private set; }

        #region 图片信息
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
        #endregion

        #region 数据访问
        /// <summary>
        /// 遍历图片
        /// </summary>
        public IEnumerable<(ConsolePixel pixel, int i, int j)> Enumerator
        {
            get
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        yield return (Data[i, j], i, j);
                    }
                }
            }
        }

        public IEnumerable<(int i, int j)> IndexEnumerator
        {
            get
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        yield return (i, j);
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
        #endregion

        #region 构造函数
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
        public static Image? Read(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }

            var lines = File.ReadAllLines(filename);

            // 获取文本最大长度
            var maxLineLength = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > maxLineLength)
                {
                    maxLineLength = lines[i].Length;
                }
            }

            var image = new Image(maxLineLength, lines.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < maxLineLength; j++)
                {
                    char c = j >= lines[i].Length ? '\0' : lines[i][j];
                    image[j, image.Height - i - 1] = c;
                }
            }

            return image;
        }
        #endregion

        public void Resize(int newWidth, int newHeight, bool clear = true)
        {
            Data = clear ? new ConsolePixel[newWidth, newHeight] : Map.GetNewSizeMap(Data, newWidth, newHeight);
            Width = newWidth;
            Height = newHeight;
        }
    }
}
