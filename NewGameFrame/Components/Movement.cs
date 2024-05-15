using GameFrame.Core;
using GameFrame.Gameplay;

namespace GameFrame.Components
{
    public class Movement : Component
    {
        public Movement(GameObject gameObject) : base(gameObject)
        {

        }

        public override void Update()
        {
            Transform.Position += Input.GetDirection();
        }
    }
}
