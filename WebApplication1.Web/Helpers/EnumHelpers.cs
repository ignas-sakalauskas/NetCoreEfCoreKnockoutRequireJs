using System;
using System.Linq;
using Newtonsoft.Json;

namespace WebApplication1.Web.Helpers
{
    /// <summary>
    /// Enum related helpers
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Helper to convert all enum's keys and values into JSON string
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>JSON formatter string</returns>
        public static string ConvertToJsonDictionary<T>()
        {
            var dictionary = Enum.GetValues(typeof(T)).Cast<object>()
                .ToDictionary(item => item.ToString(), item => (int)item);

            return JsonConvert.SerializeObject(dictionary);
        }
    }
}
