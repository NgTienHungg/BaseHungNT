using System.Collections;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;

namespace WingsMob
{
    public class BaseKeyDropdown<T> where T : BaseKeyDropdown<T>
    {
#if UNITY_EDITOR
        public static IEnumerable GetDropdown()
        {
            var result = new ValueDropdownList<string>();
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.IsLiteral && f.FieldType == typeof(string));

            foreach (var field in fields)
            {
                result.Add(field.Name, (string)field.GetValue(null));
            }

            return result;
        }
#endif
    }
}