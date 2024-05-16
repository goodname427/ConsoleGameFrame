using System.Runtime.CompilerServices;

namespace GameFrame.Core
{
    public class Object
    {
        /// <summary>
        /// 核心拷贝
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            // todo
            var type = GetType();
            
            // 浅拷贝
            var newObj = MemberwiseClone();

            // 拷贝字段
            foreach (var fieldInfo in type.GetFields())
            {
                fieldInfo.SetValue(newObj, fieldInfo.GetValue(this));
            }

            // 拷贝属性，只拷贝自动实现的属性
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.CanRead && (propertyInfo.GetGetMethod()?.IsDefined(typeof(CompilerGeneratedAttribute), true) ?? false))
                {
                    propertyInfo.SetValue(newObj, propertyInfo.GetValue(this));
                }
            }

            return newObj as Object ?? throw new InvalidCastException();
        }

        public T Clone<T>() where T : Object
        {
            return Clone() as T ?? throw new InvalidCastException();
        }
    }
}
