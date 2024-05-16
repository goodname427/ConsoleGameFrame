using GameFrame.Gameplay;
using GameFrame.Physics;
using GameFrame.Render;
using System.Diagnostics.CodeAnalysis;

namespace GameFrame.Core
{
    /// <summary>
    /// 游戏实际运行的场景，包含各式各样的组件和游戏物体
    /// </summary>
    public class Scene : Object
    {
        /// <summary>
        /// 当前场景
        /// </summary>
        public static Scene? CurrentScene { get; private set; }

        /// <summary>
        /// 场景加载时调用
        /// </summary>
        public event Action<Scene>? OnSceneLoaded;
        /// <summary>
        /// 场景更新时调用
        /// </summary>
        public event Action<Scene>? OnSceneUpdate;

        /// <summary>
        /// 游戏地图，负责渲染
        /// </summary>
        public required Map Map { get; init; }
        /// <summary>
        /// 物理系统
        /// </summary>
        public required PhysicsSystem PhysicsSystem { get; init; }

        /// <summary>
        /// 场景中所有游戏物体
        /// </summary>
        public List<GameObject> GameObjects { get; } = [];

        /// <summary>
        /// 场景中所有组件
        /// </summary>
        private readonly List<Component>[] Components = [[], [], []];

        [SetsRequiredMembers]
        public Scene()
        {
            Map = new();
            PhysicsSystem = new();
        }

        public void Start()
        {
            CurrentScene = this;
            OnSceneLoaded?.Invoke(this);
        }
        public void Update()
        {
            // 刷新地图
            Map.Clear();

            // 获取输入
            Input.GetInput();

            // 更新组件
            foreach (var components in Components)
            {
                foreach (var component in components)
                {
                    if (component.Enable)
                    {
                        if (!component.Init)
                        {
                            component.Init = true;
                            component.Start();
                        }

                        component.Update();
                    }
                }
            }

            // 物理系统更新
            PhysicsSystem.Update();

            OnSceneUpdate?.Invoke(this);
        }

        /// <summary>
        /// 组件被添加时调用
        /// </summary>
        /// <param name="newComponent"></param>
        public void OnComponentAdd(Component newComponent)
        {
            if (newComponent is Renderer renderer)
            {
                int index = 0;

                foreach (var component in Components[0])
                {
                    var _renderer = component as Renderer;
                    if (_renderer?.RenderLayer > renderer.RenderLayer)
                    {
                        Components[0].Insert(index, renderer);
                        break;
                    }

                    index++;
                }

                if (index == Components[0].Count)
                {
                    Components[0].Add(renderer);
                }
            }
            else if (newComponent is Camera camera)
            {
                Components[1].Add(camera);
            }
            else if (newComponent is Collider collider)
            {
                PhysicsSystem.AddCollider(collider);
            }
            else
            {
                Components[2].Add(newComponent);
            }
        }

        /// <summary>
        /// 寻找场景中的指定游戏物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? FindGameObjectByType<T>() where T : GameObject
        {
            foreach (var gameObject in GameObjects)
            {
                if (gameObject is T result)
                {
                    return result;
                }
            }

            return null;
        }
        /// <summary>
        /// 寻找场景中的指定组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? FindComponentByType<T>() where T : Component
        {
            foreach (var components in Components)
            {
                foreach (var component in components)
                {
                    if (component is T result)
                    {
                        return result;
                    }
                }

            }
            return null;
        }


    }
}
