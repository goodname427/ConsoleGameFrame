using GameFrame.Extra;
using System.Collections.ObjectModel;

namespace GameFrame.Core
{
    /// <summary>
    /// 游戏中的基本单元
    /// </summary>
    public class GameObject : Object
    {
        /// <summary>
        /// 游戏物体所属的场景
        /// </summary>
        public Scene OwnerScene { get; }

        /// <summary>
        /// 游戏物体的名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        #region 位置信息
        public Transform Transform { get; }
        #endregion

        #region 构造函数
        public GameObject() : this(Scene.CurrentScene, string.Empty)
        {

        }
        public GameObject(Scene? scene, string name = "", Transform? transform = null)
        {
            ArgumentNullException.ThrowIfNull(scene);

            OwnerScene = scene;
            OwnerScene.OnGameObjectAdd(this);

            Name = name;

            Transform = transform ?? new();
        }
        #endregion

        protected override void OnDestoryed()
        {
            Enumerable.Range(0, _componets.Count).Foreach(i => RemoveComponent(null, i, false, false));
            OwnerScene.OnGameObjectRemoved(this);
        }

        #region 组件
        /// <summary>
        /// 所有组件
        /// </summary>
        private readonly List<Component> _componets = [];
        /// <summary>
        /// 游戏物体带有的组件
        /// </summary>
        public ReadOnlyCollection<Component> Componets => _componets.AsReadOnly();

        private int GetComponentIndex(Component component)
        {
            return _componets.FindIndex(0, c => c == component);
        }
        private int GetComponentIndex<T>() where T : Component
        {
            return _componets.FindIndex(0, c => c is T);
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T AddComponet<T>() where T : Component
        {
            var type = typeof(T);
            if (Activator.CreateInstance(type, [this]) is not T component)
                throw new NotImplementedException(nameof(component));

            _componets.Add(component);
            OwnerScene.OnComponentAdd(component);

            return component;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetComponent<T>() where T : Component
        {
            var index = GetComponentIndex<T>();
            return index == -1 ? null : _componets[index] as T;
        }
        /// <summary>
        /// 获取所有组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetComponents<T>()
        {
            return _componets.Where(c => c is T).Cast<T>().ToArray();
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="component"></param>
        /// <param name="index"></param>
        /// <param name="removedByIndex"></param>
        /// <param name="removeComponent"></param>
        /// <returns></returns>
        private bool RemoveComponent(Component? component, int index, bool removedByIndex = true, bool removeComponent = true)
        {
            if (index == -1 && removedByIndex && removeComponent)
            {
                return false;
            }

            if (component == null && index == -1)
            {
                return false;
            }

            component ??= _componets[index];

            // 从场景中移除组件
            OwnerScene.OnComponentRemove(component);

            // 从game object中移除组件
            bool removed = true;
            if (removeComponent)
            {
                if (removedByIndex)
                {
                    _componets.RemoveAt(index);
                }
                else
                {
                    removed = _componets.Remove(component);
                }
            }

            // 销毁组件
            component.Destory();
            return removed;
        }

        /// <summary>
        /// 移除Component
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool RemoveComponent(Component component)
        {
            return RemoveComponent(component, GetComponentIndex(component));
        }
        /// <summary>
        /// 移除Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RemoveComponent<T>() where T : Component
        {
            return RemoveComponent(null, GetComponentIndex<T>());
        }
        /// <summary>
        /// remove all components of type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RemoveComponents<T>() where T : Component
        {
            var components = GetComponents<T>();
            bool removed = false;
            foreach (var component in components)
            {
                removed |= RemoveComponent(component, -1, false);
            }

            return removed;
        }
        #endregion
    }
}
