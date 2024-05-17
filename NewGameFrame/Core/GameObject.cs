using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Reflection;

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

        protected int GetComponentIndex<T>() where T : Component
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
            return component;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetComponet<T>() where T : Component
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
        /// 移除Component
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool RemoveComponent(Component component)
        {
            return _componets.Remove(component);
        }
        /// <summary>
        /// 移除Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RemoveComponent<T>() where T: Component
        {
            var index = GetComponentIndex<T>();
            if (index != -1)
            {
                _componets.RemoveAt(index);
                return true;
            }

            return false;
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
                removed |= _componets.Remove(component);
            }

            return removed;
        }
        #endregion
    }
}
