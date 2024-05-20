using GameFrame.Core.Render;
using GameFrame.Editor;

namespace GameFrame.Core
{
    public abstract class DefaultGame : IGame
    {
        public virtual int Step { get; protected set; }

        public virtual float MinDeltaTime => 0.0083f;

        public virtual Scene? GetScene(int sceneIndex)
        {
            return SceneUtils.CreatScene();
        }

        public virtual void Init()
        {
            _ = new Screen();
            Step = 0;
        }

        public virtual void Update()
        {

        }
    }
}
