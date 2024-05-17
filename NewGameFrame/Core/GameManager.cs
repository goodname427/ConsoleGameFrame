using System.Diagnostics;

namespace GameFrame.Core
{
    /// <summary>
    /// 管理整个游戏流程
    /// </summary>
    public static class GameManager
    {
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
