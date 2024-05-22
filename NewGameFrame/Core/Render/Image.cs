using System.Diagnostics.CodeAnalysis;

namespace GameFrame.Core.Render
{
    public enum ImagePivotMode
    {
        Center,
        Custom
    }

    public class Image : ISizeable
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

        private ImagePivotMode _pivotMode = ImagePivotMode.Center;
        /// <summary>
        /// 锚点模式
        /// </summary>
        public ImagePivotMode PivotMode
        {
            get => _pivotMode;
            set
            {
                if (_pivotMode != value)
                {
                    if (_pivotMode == ImagePivotMode.Center)
                    {
                        _pivot = new Vector(Width / 2, Height / 2);
                    }

                    _pivotMode = value;
                }
            }
        }

        public Vector _pivot;
        /// <summary>
        /// 图片锚点，默认为中心点
        /// </summary>
        public Vector Pivot
        {
            get => _pivot;
            set
            {
                if (PivotMode == ImagePivotMode.Custom)
                {
                    _pivot = value;
                }
            }
        }
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
        /// <summary>
        /// 图片坐标索引
        /// </summary>
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
            Data = new ConsolePixel[0, 0];
            Resize(width, height);
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

        /// <summary>
        /// 调整图片尺寸
        /// </summary>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="clear"></param>
        public void Resize(int newWidth, int newHeight, bool clear = true)
        {
            Data = clear ? new ConsolePixel[newWidth, newHeight] : Map.GetNewSizeMap(Data, newWidth, newHeight);
            Width = newWidth;
            Height = newHeight;

            if (PivotMode == ImagePivotMode.Center)
            {
                _pivot = new Vector(Width / 2, Height / 2);
            }
        }
        /// <summary>
        /// 将图片复制到指定图片上
        /// </summary>
        /// <param name="destImage"></param>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CopyTo(Image destImage, int destX = 0, int destY = 0, int startX = 0, int startY = 0, int width = 0, int height = 0, bool ignoreEmpty = true)
        {
            if (width <= 0)
            {
                width = Width;
            }
            if (height <= 0)
            {
                height = Height;
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Map.IsOutSide(destImage.Data, destX + i, destY + j) || Map.IsOutSide(Data, startX + i, startY + j) || (ignoreEmpty && this[startX + i, startY + j].IsEmpty()))
                    {
                        continue;
                    }

                    destImage[destX + i, destY + j] = this[startX + i, startY + j];
                }
            }
        }
        /// <summary>
        /// 清空图片
        /// </summary>
        public void Clear()
        {
            foreach (var (i, j) in IndexEnumerator)
            {
                Data[i, j] = ConsolePixel.Empty;
            }
        }
    }
}
