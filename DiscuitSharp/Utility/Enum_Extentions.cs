using System.ComponentModel;
using System.Reflection;

namespace DiscuitSharp.Core.Utility
{
    /// <summary>
    /// Provides extension methods for enums.
    /// </summary>
    public static class Enum_Extensions
    {
        /// <summary>
        /// Retrieves a description from an enum value. If the enum value is decorated with a DescriptionAttribute,
        /// this method returns the description specified in that attribute. Otherwise, it returns the enum's name as a string.
        /// </summary>
        /// <param name="value">The enum value from which to retrieve the description.</param>
        /// <returns>The description of the enum value if a DescriptionAttribute is applied; otherwise, the string representation of the enum value.</returns>
        /// <remarks>
        /// This method uses reflection to look up the DescriptionAttribute on the enum field. If no DescriptionAttribute is found,
        /// the method defaults to returning the standard string representation of the enum value.
        /// </remarks>
        public static string Description(this Enum value)
        {
            Type type = value.GetType();

            if (Enum.GetName(type, value) is string name)
            {
                if (type.GetField(name) is FieldInfo field)
                {
                    Attribute? attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                    if (attr is DescriptionAttribute descAttr)
                    {
                        return descAttr.Description;
                    }
                }
            }
            return value.ToString();
        }
    }
}
