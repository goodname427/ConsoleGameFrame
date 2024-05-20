using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.Components
{
    public class Movement(GameObject gameObject) : Component(gameObject)
    {
        public Vector LastDirection { get; private set; }

        public override void Update()
        {
            var dir = Input.GetDirection();

            if (dir != Vector.Zero)
            {
                LastDirection = dir;
            }

            Transform.Position += dir;
        }
    }
}
