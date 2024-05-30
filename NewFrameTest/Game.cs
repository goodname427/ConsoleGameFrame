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
            Scene? scene;
            switch (sceneIndex)
            {
                case 0:
                    scene = SceneUtils.CreatDefaultScene();
                    var canvas = new GameObject(scene, "Canvas").AddComponet<CanvasRenderer>().Canvas;

                    optionGroup = new OptionGroupElement("Hello World");
                    optionGroup
                        .AddOption(new("Start", (op) => Step = 1))
                        .AddOption(new("Option", (op) => Screen.Main!.HUD = "Game Option"))
                        .AddOption(new("Exit", (op) => Step = -1));

                    canvas.AddElement(optionGroup);
                    optionGroup.Choose();
                    return scene;
                case 1:
                    scene = SceneUtils.CreatDefaultScene();

                    // 背景
                    var backImage = Image.Read(@"C:\Users\galenglchen\Desktop\test.txt") ?? new Image();

                    var background = new GameObject(scene, transform: new(new(0, 0)));
                    background.AddComponet<ImageRenderer>().Image = backImage;
                    background.AddComponet<BoxCollider>().SetColliderToImage().IsTrigger = true;

                    // 墙
                    //var wall = new GameObject(scene, "Wall", new(new(3, 5)));
                    //wall.AddComponet<ImageRenderer>().Image = new('%', 10, 1);
                    //wall.AddComponet<BoxCollider>().SetColliderToImage();

                    // 玩家
                    var p = new GameObject(scene, "Player", new(new(0, 0)));

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

                    // Camera
                    var camera = scene.FindComponentByType<Camera>();
                    camera!.GameObject.Transform.Parent = p.Transform;
                    //camera!.RenderPasses.Add(new FilterPostProcessPass());

                    // UI
                   
                    //canvas.AddElement(new TextElement
                    //{
                    //    Position = new Vector(0, 0),
                    //    Text = "Hello, I'm ChenGuanLin"
                    //});

                    //var grid = new GridGroupElement();

                    //grid
                    //.AddElement(new TextElement { Text = "Please Select Your Favorite Food!" })
                    //.AddElement(new TextElement { Text = "1.Cola" })
                    //.AddElement(new TextElement { Text = "2.Instant noodles" })
                    //.AddElement(new TextElement { Text = "3.Crisps" })
                    //.AddElement(new TextElement { Text = "4.Shit" })
                    //;

                    //grid.GridSpacing = new Vector(50, -1);
                    //grid.MaxGridCount = 1;
                    //canvas.AddElement(grid);

                   

                    //optionGroup.Choose();

                    return scene;
                default:
                    return null;
            }
        }

        OptionGroupElement? optionGroup;

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

            if (Input.GetKey(ConsoleKey.T))
            {
                optionGroup?.Choose();
            }

            //Screen.Main!.HUD = Time.DeltaTime.ToString() + "           ";
        }
    }
}
