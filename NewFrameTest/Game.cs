using GameFrame.Components;
using GameFrame.Core;
using GameFrame.Core.Physics;
using GameFrame.Core.Render;
using GameFrame.Editor;
using GameFrame.Extra;
using GameFrame.UI;

namespace FrameTest
{
    internal class Game : DefaultGame
    {
        public override Scene? GetScene(int sceneIndex)
        {
            switch (sceneIndex)
            {
                case 0:
                    var scene = SceneUtils.CreatDefaultScene();

                    // 背景
                    var backImage = Image.Read(@"C:\Users\galenglchen\Desktop\test.txt") ?? new Image();

                    var background = new GameObject(scene, transform: new(new(0, 0)));
                    background.AddComponet<ImageRenderer>().Image = backImage;
                    background.AddComponet<BoxCollider>().SetColliderToImage().IsTrigger = true;

                    // 墙
                    //var wall = new GameObject(scene, "Wall", new(new(3, 5)));
                    //wall.AddComponet<ImageRenderer>().Image = new('%', 10, 1);
                    //wall.AddComponet<BoxCollider>().SetBoxToImage();

                    // 玩家
                    var p = new GameObject(scene, "Player", new(new(0, 0, 1)));

                    var pImgae =
                        new Image('P');
                        //new Image(new ConsolePixel[,]
                        //{
                        //    { '#', '\0', '#', '\0' },
                        //    { '\0', '#', '#' , '#'},
                        //    { '#', '\0', '#' , '\0'},
                        //});

                    pImgae.IndexEnumerator.Foreach(pos => pImgae[pos] = pImgae[pos] with { Color = ConsoleColor.Yellow });
                    p.AddComponet<Player>();
                    p.AddComponet<ImageRenderer>().Image = pImgae;
                    p.AddComponet<Movement>();
                    //p.AddComponet<CustomCollider>().SetColliderToImage().ColliderEnter += (other) => Screen.Instance!.HUD = ($"{p.Name} interact with {other.Name}");

                    var camera = scene.FindComponentByType<Camera>();
                    camera!.GameObject.Transform.Parent = p.Transform;
                    //camera!.AutoAdjustConsoleWindow = false;
                    //camera!.Width = 42;
                    //camera!.Height = 21;
                    var canvas = new GameObject(scene, "Canvas").AddComponet<Canvas>();

                    return scene;
                default:
                    return null;
            }
        }

        public override void Init()
        {
            base.Init();
            Screen.Main!.EnableDoubleWidth = false;
        }

        public override void Update()
        {
            if (Input.GetKey(ConsoleKey.Spacebar))
            {
                Screen.Main!.IsDrawFrame = !Screen.Main.IsDrawFrame;
            }

            if (Input.GetKey(ConsoleKey.P))
            {
                var go = Scene.CurrentScene?.FindGameObjectByName("Wall");
                //go?.Destory();
                go?.RemoveComponent<BoxCollider>();
            }

            if (Input.GetKey(ConsoleKey.Escape))
            {
                Step = -1;
            }

            Screen.Main!.HUD = Time.DeltaTime.ToString() + "           ";
        }
    }
}
