using GameFrame.Core;

namespace GameFrame.Render
{
    public class ImageRenderer : Renderer
    {
        public Image? Image { get; set; }

        public ImageRenderer(GameObject gameObject) : base(gameObject)
        {
            Image = new Image(0, 0);
        }

        public override void Render()
        {
            if (Image is null)
            {
                return;
            }

            DrawImageOnMap(Image);
        }
    }
}
