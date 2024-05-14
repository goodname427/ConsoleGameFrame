namespace GameFrame.Core
{
    public interface IGame
    {
        /// <summary>
        /// 游戏阶段
        /// </summary>
        int Step { get; }

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
