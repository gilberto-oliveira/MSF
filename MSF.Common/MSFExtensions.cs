using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSF.Common
{
    public static class EnumerableExtensions
    {
        public static string ToDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
        {
            return MSFEnum.GetEnumDescription(EnumValue);
        }
        public static string ToValue<TEnum>(this TEnum EnumValue) where TEnum : struct
        {
            return MSFEnum.GetEnumValue(EnumValue);
        }
    }

    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }
    }

    public static class SerializationExtensions
    {
        public static string ToJson<T>(this T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
