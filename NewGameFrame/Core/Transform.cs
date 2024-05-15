using GameFrame.MathCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Core
{
    /// <summary>
    /// 代表游戏物体的transform信息
    /// </summary>
    public class Transform : Object
    {
        private Transform? _parent;
        /// <summary>
        /// 游戏物体的父物体
        /// </summary>
        public Transform? Parent
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

        private readonly List<Transform> _child = new();
        /// <summary>
        /// 游戏物体的孩子物体
        /// </summary>
        public Transform[] Children => _child.ToArray();

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

        public Transform() : this(Vector.Zero)
        {

        }

        public Transform(Vector Position)
        {
            _position = Position;
        }
    }
}
