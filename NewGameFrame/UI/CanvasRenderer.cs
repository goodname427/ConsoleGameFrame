using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    /// <summary>
    /// 相机覆盖模式
    /// </summary>
    public enum CanvasOverrideMode
    {
        /// <summary>
        /// 覆盖到相机上
        /// </summary>
        Camera,
        /// <summary>
        /// 覆盖到世界中
        /// </summary>
        Scene
    }

    public class CanvasRenderer : Renderer
    {
        #region 渲染覆盖
        private CanvasOverrideMode _overrideMode;
        /// <summary>
        /// 相机覆盖模式
        /// </summary>
        public CanvasOverrideMode OverrideMode
        {
            get => _overrideMode;
            set
            {
                _overrideMode = value;
                if (_overrideMode != CanvasOverrideMode.Camera)
                {
                    OverridenCamera?.RenderPasses.Remove(Canvas);
                }
                else
                {
                    OverridenCamera ??= OwnerScene.FindComponentByType<Camera>();
                    if (OverridenCamera is not null)
                        OverrideCamera(OverridenCamera);
                }
            }
        }

        /// <summary>
        /// 覆盖相机
        /// </summary>
        /// <param name="camera"></param>
        private void OverrideCamera(Camera camera)
        {
            camera.RenderPasses.Add(Canvas);
        }

        private Camera? _overridenCamera = null;
        /// <summary>
        /// 覆盖相机
        /// </summary>
        public Camera? OverridenCamera
        {
            get => _overridenCamera;
            set
            {
                if (_overridenCamera != value && _overrideMode != CanvasOverrideMode.Camera)
                {
                    _overridenCamera?.RenderPasses.Remove(Canvas);
                    if (value is not null)
                    {
                        OverrideCamera(value);
                    }
                }
                _overridenCamera = value;
            }
        }
        #endregion

        #region 尺寸
        private int _width = 0;
        /// <summary>
        /// 画布宽度
        /// </summary>
        public int Width
        {
            get
            {
                return OverridenCamera?.Width ?? _width;
            }
            set
            {
                if (OverridenCamera == null)
                {
                    _width = value;
                }
            }
        }
        private int _height = 0;
        /// <summary>
        /// 画布高度
        /// </summary>
        public int Height
        {
            get
            {
                return OverridenCamera?.Height ?? _height;
            }
            set
            {
                if (OverridenCamera == null)
                {
                    _height = value;
                }
            }
        }
        #endregion

        public Canvas Canvas { get; private set; } = new();

        private readonly Image _renderCache = new();

        public CanvasRenderer(GameObject gameObject) : base(gameObject)
        {
            OverrideMode = CanvasOverrideMode.Camera;
        }

        public override void Render()
        {
            if (OverrideMode != CanvasOverrideMode.Scene) 
            {
                return;
            }

            if (_renderCache.Width != Width || _renderCache.Height != Height)
            {
                _renderCache.Resize(Width, Height);  
            }

            Canvas.PostProcess(_renderCache);

            DrawImageOnMap(_renderCache);
        }
    }
}
