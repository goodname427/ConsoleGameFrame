namespace GameFrame.Core.Render
{
    public class Camera(GameObject gameObject) : Component(gameObject)
    {
        /// <summary>
        /// 自适应相机大小为屏幕大小
        /// </summary>
        public bool AutoAdjustConsoleWindow { get; set; } = true;
        /// <summary>
        /// 相机宽度
        /// </summary>
        public int Width { get => Size.X; set => Size = Size with { X = value }; }
        /// <summary>
        /// 相机高度
        /// </summary>
        public int Height { get => Size.Y; set => Size = Size with { Y = value }; }
        /// <summary>
        /// 相机尺寸
        /// </summary>
        public Vector Size { get; set; } = Vector.Zero;

        /// <summary>
        /// 投影的屏幕
        /// </summary>
        public Screen? ProjectScreen { get; set; } = Screen.Instance;

        #region 渲染
        /// <summary>
        /// 后处理通道
        /// </summary>
        public List<ICameraRenderPass> RenderPasses { get; } = [];

        private readonly Image _renderCache = new();
        /// <summary>
        /// 获取映射的地图
        /// </summary>
        public Image RenderCache
        {
            get
            {
                // 前置检测
                ProjectScreen ??= Screen.Instance;
                ArgumentNullException.ThrowIfNull(ProjectScreen);
                ArgumentNullException.ThrowIfNull(OwnerScene);

                // 相机跟随窗口变化
                if (AutoAdjustConsoleWindow)
                {
                    if (Width != Screen.ConsoleWindowWidth - 2)
                    {
                        Width = Screen.ConsoleWindowWidth - 2;
                    }
                    if (Height != Screen.ConsoleWindowHeight - 2)
                    {
                        Height = Screen.ConsoleWindowHeight - 2;
                    }
                }

                // 扩容
                if (Map.IsOutSide(_renderCache.Data, new(Width - 1, Height - 1)))
                {
                    _renderCache.Resize(Width, Height);
                }

                // 在这里会将场景坐标系转为屏幕坐标系，场景坐标系为（x→,y↑），屏幕坐标系为（x→,y↓），y轴会进行颠倒
                var start = new Vector(Transform.Position.X - Width / 2, Transform.Position.Y + Height / 2);
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        _renderCache[i, j] = OwnerScene.Map[start.X + i, start.Y - j];
                    }
                }

                // 后处理，UI会在这里进行绘制
                foreach (var renderPass in RenderPasses)
                {
                    renderPass.RenderPass(this, _renderCache);
                }

                return _renderCache;
            }
        }

        /// <summary>
        /// 更新屏幕
        /// </summary>
        public override void Update()
        {
            if (ProjectScreen is null)
                return;

            ProjectScreen.Draw(RenderCache);
        }
        #endregion
    }
}
