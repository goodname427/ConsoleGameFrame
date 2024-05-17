﻿using GameFrame.Core;
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
        public static Scene CreatScene(int cameraWidth = 21, int cameraHeigth = 21, int cameraPostionX = 0, int cameraPositionY = 0)
        {
            var scene = new Scene();

            var go = new GameObject(scene);
            go.Transform.Position = new(cameraPostionX, cameraPositionY);
            var camera = go.AddComponet<Camera>();
            camera.Size = new Vector(cameraWidth, cameraHeigth);

            return scene;
        }
    }
}
