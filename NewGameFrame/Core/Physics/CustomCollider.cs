using GameFrame.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Core.Physics
{
    public class CustomCollider(GameObject gameObject) : Collider(gameObject)
    {
        private readonly CustomPrimitive _customPrimitive = new();

        public override IPrimitive Primitive => _customPrimitive;
    
        public void AddVertex(Vector vertex)
        {
            _customPrimitive.AddVertex(vertex);
        }

        public void RemoveVertex(Vector vertex)
        {
            _customPrimitive.RemoveVertex(vertex);
        }

        public CustomCollider SetColliderToImage(ImageRenderer? renderer = null)
        {
            renderer ??= GameObject.GetComponent<ImageRenderer>();
            if (renderer is null || renderer.Image is null)
            {
                return this;
            }

            foreach (var (pixel, i, j) in renderer.Image.Enumerator)
            {
                if (!pixel.IsEmpty())
                {
                    AddVertex(new(i - renderer.Image.Pivot.X, j - renderer.Image.Pivot.Y));
                }
            }

            return this;
        }
    }
}
