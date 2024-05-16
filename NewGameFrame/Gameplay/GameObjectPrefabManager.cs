using GameFrame.Core;
using GameFrame.Render;

namespace GameFrame.Gameplay
{
    public static class GameObjectPrefabManager
    {
        public static Camera GetCamera(Scene scene, string name = "", Transform? transform = null)
        {
            var go = new GameObject(scene, name, transform);
            var camera = go.AddComponet<Camera>();

            return camera;
        }
    }
}
