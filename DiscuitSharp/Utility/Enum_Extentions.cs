using System.ComponentModel;
using System.Reflection;

namespace DiscuitSharp.Core.Utility
{
    public static class Enum_Extentions
    {

    public static string Description(this Enum value)
        {
            // Get the type of the enum
            Type type = value.GetType();
    
            // Get the field information for this enum value
            string? name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo? field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute? attr =
                        Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
    
            // If there's no description, just return the ToString of the enum
            return value.ToString();
        }
    
    }
}
