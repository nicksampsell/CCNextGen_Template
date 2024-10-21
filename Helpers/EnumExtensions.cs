using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CCNextGen_Template.Helpers
{
    /// <summary>
    /// Extension methods for Enumerations
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Display name for an enumeration value
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            var val = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First();

            if (val == null) return enumValue.ToString();

            if (val.GetCustomAttribute<DisplayAttribute>() != null)
            {
                return val.GetCustomAttribute<DisplayAttribute>().GetName();
            }
            else
            {
                return enumValue.ToString();
            }

        }
    }
}
