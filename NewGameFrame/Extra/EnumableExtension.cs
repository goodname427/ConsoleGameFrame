using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFrame.Extra
{
    public static class EnumableExtension
    {
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> enumerable, Action<T> func)
        {
            foreach (var item in enumerable)
            {
                func(item);
            }

            return enumerable;
        }
    }
}
