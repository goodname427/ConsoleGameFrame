﻿namespace GameFrame.Core.Render
{
    public class Camera : Component
    {
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

        protected Camera(GameObject gameObject) : base(gameObject)
        {
        }

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
                ProjectScreen ??= Screen.Instance;

                if (ProjectScreen is null)
                    throw new ArgumentNullException(nameof(ProjectScreen));

                if (OwnerScene is null)
                    throw new ArgumentNullException(nameof(OwnerScene));

                if (Map.IsOutSide(_renderCache.Data, new(Width - 1, Height - 1)))
                    _renderCache.Resize(Width, Height);

                // 在这里会将场景坐标系转为屏幕坐标系，场景坐标系为（x→,y↑），屏幕坐标系为（x→,y↓）
                var start = new Vector(Transform.Position.X - Width / 2, Transform.Position.Y + Height / 2);
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        _renderCache[i, j] = OwnerScene.Map[start.X + i, start.Y - j];
                    }
                }

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