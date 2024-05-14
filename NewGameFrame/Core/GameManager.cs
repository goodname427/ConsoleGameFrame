using GameFrame.MathCore;
using GameFrame.Render;
using System.Security.Cryptography;

namespace GameFrame.Core
{
    public static class GameManager
    {
        /// <summary>
        /// 创建一个默认场景，包含一个相机
        /// </summary>
        /// <param name="width"></param>
        /// <param name="heigth"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Scene CreatScene(int width = 21, int heigth = 21, int x = 0, int y = 0)
        {
            var scene = new Scene();
            var go = new GameObject(scene) { Position = new(x, y) };
            var camera = go.AddComponet<Camera>();
            camera.Size = new Vector(width, heigth);
            return scene;
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="game"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Start(IGame game)
        {
            game.Init();
            var step = -1;
            Scene? scene = null;
            while (game.Step >= 0)
            {
                //更换场景
                if (step != game.Step || scene is null)
                {
                    scene = game.GetScene(game.Step);
                    if (scene is null)
                    {
                        throw new ArgumentNullException(nameof(scene));
                    }

                    step = game.Step;
                    scene.Start();
                }
                //场景更新
                scene.Update();
                game.Update();
            }
        }
    }
}
