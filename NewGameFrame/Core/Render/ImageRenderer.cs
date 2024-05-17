namespace GameFrame.Core.Render
{
    public class ImageRenderer(GameObject gameObject) : Renderer(gameObject)
    {
        public Image? Image { get; set; } = new Image(0, 0);

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
