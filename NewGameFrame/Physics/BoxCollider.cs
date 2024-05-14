using GameFrame.Core;
using GameFrame.MathCore;
using GameFrame.Render;

namespace GameFrame.Physics
{
    public class BoxCollider : Collider
    {
        private readonly BoxPrimitive _box = new();
        public Vector Min
        { 
            get => _box.Min; 
            set => _box.Min = value;
        }

        public Vector Max
        {
            get => _box.Max;
            set => _box.Max = value;
        }

        public override IPrimitive Primitive => _box;

        public BoxCollider(GameObject gameObject) : base(gameObject)
        {
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="renderer"></param>
        public BoxCollider SetBoxToImage(ImageRenderer? renderer = null)
        {
            renderer ??= GameObject.GetComponet<ImageRenderer>();
            if (renderer is null || renderer.Image is null)
            {
                return this;
            }

            Min = -renderer.Image.Pivot;
            Max = new Vector(renderer.Image.Width - 1, renderer.Image.Height - 1) - renderer.Image.Pivot;
        
            return this;
        }
    }
}
