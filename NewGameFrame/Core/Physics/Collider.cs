namespace GameFrame.Core.Physics
{
    public abstract class Collider(GameObject gameObject) : Component(gameObject)
    {
        public delegate void ColliderEventHandler(Collider other);

        public event ColliderEventHandler? ColliderEnter;
        public event ColliderEventHandler? ColliderExit;
        public event ColliderEventHandler? ColliderSaty;

        public abstract IPrimitive Primitive { get; }

        public sealed override void Start() { }
        public sealed override void Update() { }

        public void OnColliderEnter(Collider other)
        {
            ColliderEnter?.Invoke(other);
        }

        public void OnColliderStay(Collider other)
        {
            ColliderSaty?.Invoke(other);
        }

        public void OnColliderExit(Collider other)
        {
            ColliderExit?.Invoke(other);
        }
    }
}
