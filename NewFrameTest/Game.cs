using GameFrame.MathCore;
using GameFrame.Core;
using GameFrame.Render;
using GameFrame.Physics;
using GameFrame.Components;
using GameFrame.UI;

namespace FrameTest
{
    internal class Game : IGame
    {
        public int Step { get; set; }

        public Scene? GetScene(int sceneIndex)
        {
            switch (sceneIndex)
            {
                case 0:
                    var scene = GameManager.CreatScene(21, 21);

                    // 背景
                    var image = new Image(21, 21);
                    var r = new Random();
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j =0; j < image.Height; j++)
                        {
                            image[i, j] = '.';
                        }
                    }

                    _ = new GameObject(scene) { Position = new(0, 0) }.AddComponet<ImageRenderer>().Image = image;
                    
                    // 墙
                    var wall = new GameObject(scene, "Wall") { Position = new(3, 5) };
                    wall.AddComponet<ImageRenderer>().Image = new('%', 10, 1);
                    wall.AddComponet<BoxCollider>().SetBoxToImage();

                    // 玩家
                    var p = new GameObject(scene, "Player") { Position = new Vector(0, 0, 1) };
                    p.AddComponet<ImageRenderer>().Image = new(new('#', ConsoleColor.Yellow, ConsoleColor.Red), 3, 3);
                    p.AddComponet<Movement>();
                    p.AddComponet<BoxCollider>().SetBoxToImage().ColliderEnter += (other) => Screen.Instance!.HUD = ($"{p.Name} interact with {other.Name}");

                    scene.FindComponentByType<Camera>()!.GameObject.Parent = p;

                    var canvas = new GameObject(scene, "Canvas").AddComponet<Canvas>();
                    
                    return scene;
                default:
                    return null;
            }
        }

        public void Init()
        {
            _ = new Screen();
            Step = 0;
        }

        public void Update()
        {
            if (Input.CurrentInput == ConsoleKey.Spacebar)
            {
                Screen.Instance!.IsDrawFrame = !Screen.Instance.IsDrawFrame;
            }
        }
    }
}
