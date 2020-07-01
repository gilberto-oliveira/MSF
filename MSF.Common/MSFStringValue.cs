using System;

namespace MSF.Common
{
    public static class MSFStringValue
    {
        public static string GetDescription(Type typ, object i)
        {
            return GetStringValueAttribute(typ, i).Description;
        }

        public static string GetStringValue(Type typ, object i)
        {
            return GetStringValueAttribute(typ, i).StringValue;
        }

        private static StringValueAttribute GetStringValueAttribute(Type typ, object i)
        {
            var name = Enum.GetName(typ, i);
            var desc = name;
            var fi = typ.GetField(name);

            var attributes = (StringValueAttribute[])fi.GetCustomAttributes(typeof(StringValueAttribute), false);

            return attributes.Length <= 0 ? new StringValueAttribute(desc) : attributes[0];
        }
    }

    public class StringValueAttribute : Attribute
    {
        public string Description { get; set; }

        public string StringValue { get; set; }
        
        public StringValueAttribute(string value, string charValue = null)
        {
            Description = value;
            StringValue = charValue;
        }
    }
}