using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    public class Canvas : Renderer
    {
        #region 类声明
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
        /// <summary>
        /// UI渲染通道
        /// </summary>
        class CanvasCameraRenderPass(Canvas canvas) : ICameraRenderPass
        {
            public Canvas Canvas { get; } = canvas;

            public void RenderPass(Camera camera, Image renderCache)
            {
                foreach (var (pixel, i, j) in Canvas.RenderCache.Enumerator)
                {
                    if (pixel.IsEmpty())
                    {
                        continue;
                    }

                    renderCache[i, j] = pixel;
                }
            }
        }
        #endregion

        public Canvas(GameObject gameObject) : base(gameObject)
        {
            _cameraRenderPass = new(this);
            OverrideMode = CanvasOverrideMode.Camera;
        }

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
                    OverridenCamera?.RenderPasses.Remove(_cameraRenderPass);
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
            camera.RenderPasses.Add(_cameraRenderPass);
            ;
        }

        /// <summary>
        /// 相机后处理管线
        /// </summary>
        private readonly CanvasCameraRenderPass _cameraRenderPass;

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
                    _overridenCamera?.RenderPasses.Remove(_cameraRenderPass);
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


        private readonly Image _renderCache = new();
        private Image RenderCache
        {
            get
            {
                if (_renderCache.Height != Height || _renderCache.Width != Width)
                {
                    _renderCache.Resize(Width, Height, true);
                }

                string text = "Hello, I'm ChenGuanLin!";
                for (int i = 0; i < text.Length && i < Width; i++)
                {
                    _renderCache[i, Height - 1] = new(text[i], ConsoleColor.Black, ConsoleColor.White);
                }
                return _renderCache;
            }
        }

        public override void Render()
        {
            if (OverrideMode != CanvasOverrideMode.Scene)
            {
                return;
            }

            DrawImageOnMap(RenderCache);
        }
    }
}
