using GameFrame.MathCore;
using GameFrame.Render;
using System.Diagnostics;
using System.Security.Cryptography;

namespace GameFrame.Core
{
    /// <summary>
    /// 管理整个游戏流程
    /// </summary>
    public static class GameManager
    {
        /// <summary>
        /// 创建一个默认场景，包含一个相机
        /// </summary>
        /// <param name="cameraWidth"></param>
        /// <param name="cameraHeigth"></param>
        /// <param name="cameraPostionX"></param>
        /// <param name="cameraPositionY"></param>
        /// <returns></returns>
        public static Scene CreatScene(int cameraWidth = 21, int cameraHeigth = 21, int cameraPostionX = 0, int cameraPositionY = 0)
        {
            var scene = new Scene();

            var go = new GameObject(scene);
            go.Transform.Position = new(cameraPostionX, cameraPositionY);
            var camera = go.AddComponet<Camera>();
            camera.Size = new Vector(cameraWidth, cameraHeigth);
            
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

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            float lastTime = 0;
            while (game.Step >= 0)
            {
                //更换场景
                if (step != game.Step || scene is null)
                {
                    scene = game.GetScene(game.Step);
                    if (scene is null)
                    {
                        throw new NullReferenceException(nameof(scene));
                    }

                    step = game.Step;
                    scene.Start();
                }

                //场景更新
                scene.Update();
                game.Update();

                // 更新游戏时间
                Time.TotalTime = stopwatch.ElapsedMilliseconds * 1000;
                Time.DeltaTime = Time.TotalTime - lastTime;
                lastTime = Time.TotalTime;

                // 限制帧时长
                if (Time.DeltaTime < game.MinDeltaTime)
                {
                    Thread.Sleep((int)((game.MinDeltaTime - Time.DeltaTime) * 1000));
                }
            }
        }
    }
}
