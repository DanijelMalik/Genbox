using System;
using System.Collections.Generic;
using System.Linq;
using Generator4Developers.Presentation.Windows.Properties;

namespace Generator4Developers.Presentation.Windows.Common
{
    public class EnumHelper
    {
        public static IDictionary<T, string> ToDictionary<T>()
        {
            var enumType = typeof(T);
            var dictionary = Enum.GetValues(enumType)
                .Cast<T>()
                .ToDictionary(x => x, TranslateResource);

            return dictionary;
        }

        private static string TranslateResource<T>(T value)
        {
            var enumType = typeof(T);
            var defaultValue = value.ToString();
            var translation = Resources.ResourceManager.GetString(String.Format("Enum_{0}_{1}", enumType.Name, defaultValue));
            return String.IsNullOrEmpty(translation) ? defaultValue : translation;
        }

        public static IDictionary<int, string> ToDictionaryInt<T>()
        {
            var enumType = typeof(T);
            var dictionary = Enum.GetValues(enumType)
                .Cast<T>()
                .ToDictionary(x => Convert.ToInt32(x), TranslateResource);

            return dictionary;
        }
    }
}