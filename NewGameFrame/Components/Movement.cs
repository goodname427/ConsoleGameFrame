using GameFrame.Core;
using GameFrame.Gameplay;

namespace GameFrame.Components
{
    public class Movement(GameObject gameObject) : Component(gameObject)
    {
        public override void Update()
        {
            Transform.Position += Input.GetDirection();
        }
    }
}
