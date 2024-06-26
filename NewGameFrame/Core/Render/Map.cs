﻿namespace GameFrame.Core.Render
{
    public class Map
    {
        /// <summary>
        /// 尝试获取地图图像
        /// </summary>
        /// <param name="quadrant"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        protected static ConsolePixel TryGetValue(ConsolePixel[,] quadrant, Vector position)
        {
            return IsOutSide(quadrant, position) ? ConsolePixel.Empty : quadrant[position.X, position.Y];
        }
        /// <summary>
        /// 获取位置所在象限索引
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected static int GetQuadrantIndexAndPosition(ref Vector position)
        {
            if (position.X >= 0 && position.Y >= 0)
            {
                return 0;
            }
            else if (position.X < 0 && position.Y >= 0)
            {
                position.X = Math.Abs(position.X) - 1;
                return 1;
            }
            else if (position.X < 0 && position.Y < 0)
            {
                position.X = Math.Abs(position.X) - 1;
                position.Y = Math.Abs(position.Y) - 1;
                return 2;
            }
            else
            {
                position.Y = Math.Abs(position.Y) - 1;
                return 3;
            }
        }
        /// <summary>
        /// 获取新尺寸的地图
        /// </summary>
        /// <param name="map"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static ConsolePixel[,] GetNewSizeMap(ConsolePixel[,] map, int newWidth, int newHeight)
        {
            var (width, height) = (map.GetLength(0), map.GetLength(1));
            if (width == newWidth && height == newHeight)
                return map;

            var newMap = new ConsolePixel[newWidth, newHeight];
            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    newMap[i, j] = IsOutSide(map, new(i, j)) ? ConsolePixel.Empty : map[i, j];
                }
            }
            return newMap;
        }
        /// <summary>
        /// 判断位置是否越界
        /// </summary>
        /// <param name="quadrant"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsOutSide(ConsolePixel[,] quadrant, int x, int y)
        {
            var (width, height) = (quadrant.GetLength(0), quadrant.GetLength(1));
            return x >= width || y >= height || x < 0 || y < 0;
        }
        /// <summary>
        /// 判断位置是否越界
        /// </summary>
        /// <param name="quadrant"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsOutSide(ConsolePixel[,] quadrant, Vector position)
        {
            return IsOutSide(quadrant, position.X, position.Y);
        }

        private readonly ConsolePixel[][,] _quadrant = [new ConsolePixel[0, 0], new ConsolePixel[0, 0], new ConsolePixel[0, 0], new ConsolePixel[0, 0]];

        public ConsolePixel this[Vector position]
        {
            get
            {
                var index = GetQuadrantIndexAndPosition(ref position);
                //return TryGetValue(_quadrant[index], position);
                return IsOutSide(_quadrant[index], position) ? ConsolePixel.Empty : _quadrant[index][position.X, position.Y];
            }
            set
            {
                var index = GetQuadrantIndexAndPosition(ref position);

                if (IsOutSide(_quadrant[index], position))
                {
                    _quadrant[index] = GetNewSizeMap(_quadrant[index], Math.Max(_quadrant[index].GetLength(0), position.X + 1), Math.Max(_quadrant[index].GetLength(1), position.Y + 1));
                }

                _quadrant[index][position.X, position.Y] = value;
            }
        }
        public ConsolePixel this[int x, int y]
        {
            get => this[new Vector(x, y)];
            set => this[new Vector(x, y)] = value;
        }

        /// <summary>
        /// 获取指定象限
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ConsolePixel[,] GetQuadrant(int index) => _quadrant[index];
        /// <summary>
        /// 清理地面
        /// </summary>
        public void Clear()
        {
            foreach (var quadrant in _quadrant)
            {
                for (int i = 0; i < quadrant.GetLength(0); i++)
                {
                    for (int j = 0; j < quadrant.GetLength(1); j++)
                    {
                        quadrant[i, j] = ConsolePixel.Empty;
                    }
                }
            }
        }

    }
}
