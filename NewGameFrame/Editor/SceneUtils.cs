using GameFrame.Core;
using GameFrame.Core.Render;

namespace GameFrame.Editor
{
    public static class SceneUtils
    {
        /// <summary>
        /// 创建一个默认场景，包含一个相机
        /// </summary>
        /// <param name="cameraWidth"></param>
        /// <param name="cameraHeigth"></param>
        /// <param name="cameraPostionX"></param>
        /// <param name="cameraPositionY"></param>
        /// <returns></returns>
        public static Scene CreatDefaultScene()
        {
            var scene = new Scene();

            var go = new GameObject(scene);
            go.AddComponet<Camera>();

            return scene;
        }
    }
}
