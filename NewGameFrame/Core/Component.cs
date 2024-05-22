namespace GameFrame.Core
{
    /// <summary>
    /// 组件，可以挂载在游戏物体上，赋予游戏物体一定的功能
    /// </summary>
    public abstract class Component(GameObject gameObject) : Object
    {
        /// <summary>
        /// 组件所属gameobject
        /// </summary>
        public GameObject GameObject { get; private set; } = gameObject;
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

        /// <summary>
        /// !!请不要手动调用该函数!! <br/>
        /// 如果需要销毁组件请使用 
        /// <code>
        /// GameObject.RemoveComponent(this)
        /// </code>
        /// 或者
        /// <code>
        /// Remove()
        /// </code>
        /// </summary>
        public sealed override void Destory()
        {
            base.Destory();
        }
        /// <summary>
        /// 移除组件
        /// </summary>
        public void Remove()
        {
            GameObject.RemoveComponent(this);
        }

        /// <summary>
        /// 组件是否初始化
        /// </summary>
        public bool Init { get; set; } = false;
        public virtual void Update() { }
        public virtual void Start() { }
    }
}
