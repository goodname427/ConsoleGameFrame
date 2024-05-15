using GameFrame.Core;
using GameFrame.MathCore;
using System;
using System.Diagnostics;
using System.Net;

namespace GameFrame.Render
{
    public class Renderer : Component
    {
        public int RenderLayer;

        public Renderer(GameObject gameObject) : base(gameObject)
        {

        }

        /// <summary>
        /// 绘制图片到地图上
        /// </summary>
        /// <param name="image"></param>
        protected void DrawImageOnMap(Image image)
        {
            foreach (var (pixel, x, y) in image.Enumerator)
            {
                if (pixel.IsEmpty())
                {
                    continue;
                }

                OwnerScene.Map[x + Transform.Position.X - image.Pivot.X, y + Transform.Position.Y - image.Pivot.Y] = image[x, y];
            }
        }

        /// <summary>
        /// 渲染
        /// </summary>
        public virtual void Render() { }

        public override void Update()
        {
            Render();
        }
    }
}
