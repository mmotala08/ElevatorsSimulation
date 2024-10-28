using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Enums.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            // Get the type of the enum
            Type enumType = value.GetType();

            // Get the member info for the specific enum value
            MemberInfo[] memberInfo = enumType.GetMember(value.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                // Try to get the DescriptionAttribute from the enum value
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            // If no description is found, return the enum value's name
            return value.ToString();
        }
    }
}
