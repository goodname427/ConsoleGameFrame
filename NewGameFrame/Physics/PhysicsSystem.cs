using GameFrame.MathCore;

namespace GameFrame.Physics
{
    public class PhysicsSystem
    {
        /// <summary>
        /// 判断另一个几何体是否在几何体内
        /// </summary>
        /// <param name="primitive"></param>
        /// <param name="otherPrimitive"></param>
        /// <returns></returns>
        public static bool IsInside(Collider collider, Collider otherCollider)
        {
            foreach (var vertex in otherCollider.Primitive.Vertexes)
            {
                if (collider.Primitive.IsInside(vertex + otherCollider.Transform.Position - collider.Transform.Position))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断两个几何体是否相交
        /// </summary>
        /// <param name="primitive1"></param>
        /// <param name="primitive2"></param>
        /// <returns></returns>
        public static bool Interact(Collider collider1, Collider collider2)
        {
            return IsInside(collider1, collider2) || IsInside(collider2, collider1);
        }

        /// <summary>
        /// 场景中的碰撞体
        /// </summary>
        public readonly List<Collider> Colliders = [];

        private readonly Dictionary<Collider, Vector> ColliderPositionCache = [];

        public void AddCollider(Collider newCollider)
        {
            Colliders.Add(newCollider);
            ColliderPositionCache.Add(newCollider, newCollider.Transform.Position);
        }

        public void Update()
        {
            for (int i = 0; i < Colliders.Count; i++)
            {
                var a = Colliders[i];
                var aPos = ColliderPositionCache[a];
                for (int j = 0; j < i; j++)
                {
                    var b = Colliders[j];
                    var bPos = ColliderPositionCache[b];

                    if (Interact(a, b))
                    {
                        a.OnColliderEnter(b);
                        b.OnColliderEnter(a);

                        // 回退位置
                        if (a.Transform.Position != aPos)
                        {
                            a.Transform.Position = aPos;
                        }
                        if (b.Transform.Position != bPos)
                        {
                            b.Transform.Position = bPos;
                        }
                    }
                }
            }

            foreach (var collider in Colliders)
            {
                if (collider.Transform.Position != ColliderPositionCache[collider])
                {
                    ColliderPositionCache[collider] = collider.Transform.Position;
                }
            }
        }
    }
}
