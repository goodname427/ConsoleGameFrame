using GameFrame.Components;
using GameFrame.Core;
using GameFrame.Core.Physics;
using GameFrame.Core.Render;

namespace FrameTest
{
    public class Player(GameObject gameObject) : Component(gameObject)
    {
        public override void Update()
        {
            if (Input.GetKey(ConsoleKey.J))
            {
                var go = new GameObject();
                go.Transform.Position = Transform.Position;
                go.AddComponet<ImageRenderer>().Image = new(new ConsolePixel('O', ConsoleColor.Red));
                go.AddComponet<BoxCollider>().SetColliderToImage();
                go.AddComponet<Bullet>().Direction = GameObject.GetComponent<Movement>()?.LastDirection ?? Vector.Zero;
                go.ToString();
            }
        }
    }
}
