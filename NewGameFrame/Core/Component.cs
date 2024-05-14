using GameFrame.MathCore;

namespace GameFrame.Core
{
    public abstract class Component
    {
        public bool Init = false;

        /// <summary>
        /// 组件所属gameobject
        /// </summary>
        public GameObject GameObject { get; set; }

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
        public Vector Position
        {
            get => GameObject.Position;
            set => GameObject.Position = value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        public Component(GameObject gameObject)
        {
            GameObject = gameObject;
            OwnerScene.OnComponentAdd(this);
        }

        public virtual void Update() { }
        public virtual void Start() { }
    }
}
