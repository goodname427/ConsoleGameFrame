namespace GameFrame.Core
{
    /// <summary>
    /// 代表游戏物体的transform信息
    /// </summary>
    public class Transform(Vector position) : Object
    {
        private Transform? _parent;
        /// <summary>
        /// 游戏物体的父物体
        /// </summary>
        public virtual Transform? Parent
        {
            get => _parent;
            set
            {
                Transform? parent = value;
                while (parent != null)
                {
                    if (parent == this)
                    {
                        return;
                    }
                    parent = parent.Parent;
                }

                if (_parent != value)
                {
                    _parent?._children.Remove(this);
                    value?._children.Add(this);
                    _parent = value;
                }
            }
        }

        protected readonly List<Transform> _children = [];
        /// <summary>
        /// 游戏物体的孩子物体
        /// </summary>
        public Transform[] Children => [.. _children];

        protected Vector _position = position;
        /// <summary>
        /// 游戏物体的位置
        /// </summary>
        public virtual Vector Position
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

        public Transform() : this(Vector.Zero)
        {

        }
    }
}
