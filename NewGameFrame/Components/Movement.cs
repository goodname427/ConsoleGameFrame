using GameFrame.Core;

namespace GameFrame.Components
{
    public class Movement : Component
    {
        public Movement(GameObject gameObject) : base(gameObject)
        {

        }

        public override void Update()
        {
            Position += Input.GetDirection();
        }
    }
}
