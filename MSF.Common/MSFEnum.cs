using MSF.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MSF.Common
{
    public class MSFEnum
    {
        public static IList<MSFEnumModel> GetEnum<T>()
        {
            var enus = Enum.GetValues(typeof(T)).Cast<T>();
            return enus.ToList().Select(source => new MSFEnumModel
            {
                Status = MSFStringValue.GetStringValue(source.GetType(), source) ?? source.GetHashCode().ToString(CultureInfo.InvariantCulture),
                Description = MSFStringValue.GetDescription(source.GetType(), source),
                Enumerator = source
            }).ToList();
        }

        public static string GetEnumValue<T>(T t)
        {
            return GetEnum<T>().FirstOrDefault(r => r.Enumerator.Equals(t)).Status;
        }

        public static string GetEnumDescription<T>(T t)
        {
            return GetEnum<T>().FirstOrDefault(r => r.Enumerator.Equals(t)).Description;
        }

        public static string GetEnumDescription<T>(string value)
        {
            return GetEnum<T>().FirstOrDefault(r => r.Status.Equals(value)).Description;
        }

        public static T GetEnumEnumarator<T>(string value)
        {
            return GetEnum<T>().FirstOrDefault(r => r.Status.Equals(value)).Enumerator;
        }

        public static T GetEnumEnumarator<T>(int value)
        {
            return GetEnum<T>().FirstOrDefault(r => r.Enumerator.GetHashCode().Equals(value)).Enumerator;
        }
    }

    public enum MSFEnumClaims
    {
        [StringValue("full_name")]
        FullName,
        [StringValue("enviroment")]
        Enviroment
    }

    public enum MSFEnumDefaults
    {
        [StringValue("Msf+123")]
        Password
    }
}
