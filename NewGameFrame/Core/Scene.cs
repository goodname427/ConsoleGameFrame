using GameFrame.Core.Physics;
using GameFrame.Core.Render;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

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

        private readonly List<GameObject> _gameObjects = [];
        /// <summary>
        /// 场景中所有游戏物体
        /// </summary>
        public GameObject[] GameObjects => [.. _gameObjects];

        private readonly List<Component>[] _components = [[], [], []];
        /// <summary>
        /// 场景中所有组件
        /// Renderer, Camera, Others
        /// </summary>
        private Component[][] Components => _components.Select(list => list.ToArray()).ToArray();

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

        #region 物体管理
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
                        _components[0].Insert(index, renderer);
                        break;
                    }

                    index++;
                }

                if (index == _components[0].Count)
                {
                    _components[0].Add(renderer);
                }
            }
            else if (newComponent is Camera camera)
            {
                _components[1].Add(camera);
            }
            else if (newComponent is Collider collider)
            {
                PhysicsSystem.AddCollider(collider);
                _components[2].Add(collider);
            }
            else
            {
                _components[2].Add(newComponent);
            }
        }
        /// <summary>
        /// 组件被移除时调用
        /// </summary>
        /// <param name="removedComponent"></param>
        public void OnComponentRemove(Component removedComponent)
        {
            if (removedComponent is Renderer)
            {
                _components[0].Remove(removedComponent);
            }
            else if (removedComponent is Camera)
            {
                _components[1].Remove(removedComponent);
            }
            else if (removedComponent is Collider collider)
            {
                PhysicsSystem.RemoveCollider(collider);
                _components[2].Remove(collider);
            }
            else
            {
                _components[2].Remove(removedComponent);
            }
        }

        /// <summary>
        /// 游戏物体被创建时调用
        /// </summary>
        /// <param name="newGameObject"></param>
        public void OnGameObjectAdd(GameObject newGameObject)
        {
            _gameObjects.Add(newGameObject);
        }
        /// <summary>
        /// 游戏物体被移除时调用
        /// </summary>
        /// <param name="removedGameObject"></param>
        public void OnGameObjectRemoved(GameObject removedGameObject)
        {
            _gameObjects.Remove(removedGameObject);
        }
        #endregion

        #region 寻找物体
        /// <summary>
        /// 寻找场景中的指定游戏物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? FindGameObjectByType<T>() where T : GameObject
        {
            return GameObjects.FirstOrDefault(x => x is T) as T;
        }
        public GameObject? FindGameObjectByName(string name)
        {
            return GameObjects.FirstOrDefault(x => x.Name == name);
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
        
        #endregion
    }
}
