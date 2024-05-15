namespace GameFrame.Core
{
    /// <summary>
    /// 游戏接口，实现该接口即可实现一个游戏
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// 游戏阶段
        /// </summary>
        int Step { get; }

        /// <summary>
        /// 最大FPS
        /// </summary>
        float MinDeltaTime { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 游戏更新
        /// </summary>
        void Update();
        /// <summary>
        /// 获得场景
        /// </summary>
        /// <param name="sceneIndex"></param>
        /// <returns></returns>
        Scene? GetScene(int sceneIndex);
    }
}
