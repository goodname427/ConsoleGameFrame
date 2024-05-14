using GameFrame.MathCore;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace GameFrame.Core
{
    public class GameObject
    {
        /// <summary>
        /// 游戏物体所属的场景
        /// </summary>
        public Scene OwnerScene { get; set; }

        /// <summary>
        /// 游戏物体的名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        #region 位置信息
        private GameObject? _parent;
        /// <summary>
        /// 游戏物体的父物体
        /// </summary>
        public GameObject? Parent
        {
            get => _parent;
            set
            {
                if (_parent?.Parent == this)
                {
                    return;
                }

                if (_parent != value)
                {
                    _parent?._child.Remove(this);
                    value?._child.Add(this);
                    _parent = value;
                }
            }
        }

        private readonly List<GameObject> _child = new();
        /// <summary>
        /// 游戏物体的孩子物体
        /// </summary>
        public GameObject[] Children => _child.ToArray();

        private Vector _position = Vector.Zero;
        /// <summary>
        /// 游戏物体的位置
        /// </summary>
        public Vector Position
        {
            get => _position;
            set
            {
                var offset = value - _position;

                if (offset == Vector.Zero)
                {
                    return;
                }

                foreach (var child in Children)
                {
                    child.Position += offset;
                }

                _position = value;
            }
        }
        /// <summary>
        /// 局部位置
        /// </summary>
        public Vector LocalPosition
        {
            get => Position - Parent?.Position ?? Vector.Zero;
            set => Position = value + Parent?.Position ?? Vector.Zero;
        }
        #endregion

        #region 构造函数
        public GameObject() : this(Scene.CurrentScene, string.Empty)
        {

        }
        public GameObject(Scene? scene, string name = "")
        {
            if (scene is null)
                throw new ArgumentNullException(nameof(scene));

            OwnerScene = scene;
            scene.GameObjects.Add(this);

            Name = name;
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
