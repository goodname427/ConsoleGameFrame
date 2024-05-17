namespace GameFrame.Core
{
    /// <summary>
    /// 组件，可以挂载在游戏物体上，赋予游戏物体一定的功能
    /// </summary>
    public abstract class Component : Object
    {
        public bool Init = false;

        /// <summary>
        /// 组件所属gameobject
        /// </summary>
        public GameObject GameObject { get; private set; }

        /// <summary>
        /// 组件所属场景
        /// </summary>
        public Scene OwnerScene => GameObject.OwnerScene;

        /// <summary>
        /// 组件所属gameobject的名称
        /// </summary>
        public string Name
        {
            get => GameObject.Name;
            set => GameObject.Name = value;
        }
        /// <summary>
        /// 组件所属gameobject位置
        /// </summary>
        public Transform Transform => GameObject.Transform;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        protected Component(GameObject gameObject)
        {
            GameObject = gameObject;
            OwnerScene.OnComponentAdd(this);
        }

        protected override void OnDestoryed()
        {
            OwnerScene.OnComponentRemove(this);
        }

        public virtual void Update() { }
        public virtual void Start() { }
    }
}
