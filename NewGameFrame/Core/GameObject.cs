using GameFrame.MathCore;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

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
            if (scene is null)
                throw new ArgumentNullException(nameof(scene));

            OwnerScene = scene;
            scene.GameObjects.Add(this);

            Name = name;

            Transform = transform ?? new();
        }
        #endregion

        #region 组件
        /// <summary>
        /// 所有组件
        /// </summary>
        private readonly List<Component> _componets = new();
        /// <summary>
        /// 游戏物体带有的组件
        /// </summary>
        public ReadOnlyCollection<Component> Componets => _componets.AsReadOnly();

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T AddComponet<T>() where T : Component
        {
            var type = typeof(T);
            if (type.GetConstructor(new Type[] { typeof(GameObject) })?.Invoke(new object[] { this }) is not T component)
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
            return _componets.FirstOrDefault(c => c is T) as T;
        }
        #endregion
    }
}
