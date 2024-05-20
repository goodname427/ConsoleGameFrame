using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.UI
{
    public class Canvas : Renderer
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

        class CanvasCameraRenderPass : ICameraRenderPass
        {
            public Canvas Canvas { get; }

            public CanvasCameraRenderPass(Canvas canvas)
            {
                Canvas = canvas;
            }

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

        public Canvas(GameObject gameObject) : base(gameObject)
        {
            _cameraRenderPass = new(this);
            OverrideMode = CanvasOverrideMode.Camera;
        }

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

        void OverrideCamera(Camera camera)
        {
            camera.RenderPasses.Add(_cameraRenderPass);
            _renderCache.Resize(camera.Width, camera.Height);
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



        private readonly Image _renderCache = new();
        private Image RenderCache
        {
            get
            {
                string text = "你好，我是陈冠霖";
                for (int i = 0; i < text.Length; i++)
                {
                    _renderCache[i, 0] = text[i];
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
