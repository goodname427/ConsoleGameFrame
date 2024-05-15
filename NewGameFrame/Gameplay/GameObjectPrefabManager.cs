using GameFrame.Core;
using GameFrame.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Gameplay
{
    public static class GameObjectPrefabManager
    {
        public static GameObject GetCamera(Scene scene, string name = "", Transform? transform = null)
        {
            var go = new GameObject(scene, name, transform);
            go.AddComponet<Camera>();

            return go;
        }
    }
}
