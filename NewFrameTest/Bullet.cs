using GameFrame.Core;
using GameFrame.Core.Physics;
using GameFrame.Core.Render;

namespace FrameTest
{
    public class Bullet(GameObject gameObject) : Component(gameObject)
    {
        public Vector Direction { get; set; }

        public override void Start()
        {
            GameObject.GetComponent<Collider>()!.ColliderEnter += OnColliderEnter;
        }

        bool _a = false;

        public override void Update()
        {
            _a = !_a;
            if (_a)
            {
                Transform.Position += Direction;
            }

            if (Transform.Position.Length > 100)
            {
                GameObject.Destory();
            }
        }

        public void OnColliderEnter(Collider collider)
        {
            if (collider.GameObject == GameObject)
            {
                return;
            }

            var imageRenderer = collider.GameObject.GetComponent<ImageRenderer>();

            if (imageRenderer == null || imageRenderer.Image == null)
            {
                return;
            }

            var relative = Transform.Position - collider.Transform.Position;
            imageRenderer.Image[relative + imageRenderer.Image.Pivot] = '\0';
        }


    }
}
