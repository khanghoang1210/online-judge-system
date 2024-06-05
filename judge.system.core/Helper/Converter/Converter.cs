using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace judge.system.core.Helper.Converter
{
    public class Converter
    {
        public static string ToPascalCase(string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = textInfo.ToTitleCase(words[i]);
            }
            return string.Concat(words);
        }

        public static string DictionaryValuesToString(Dictionary<dynamic, dynamic> dictionary)
        {
            StringBuilder outputString = new StringBuilder();
            foreach (var pair in dictionary)
            {
                if (pair.Value is string)
                {
                    outputString.Append("\"").Append(pair.Value).Append("\", ");
                }
                else
                {
                    outputString.Append(pair.Value).Append(", ");
                }
            }
            if (outputString.Length > 0)
            {
                outputString.Length -= 2;
            }
            return outputString.ToString();
        }
    }
    public class DynamicDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Dictionary<string, dynamic>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dictionary = new Dictionary<dynamic, dynamic>();
            var jsonObject = JObject.Load(reader);

            foreach (var property in jsonObject.Properties())
            {
                dictionary[property.Name] = property.Value.ToObject<dynamic>();
            }

            return dictionary;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dictionary = (Dictionary<string, dynamic>)value;
            var jsonObject = new JObject();

            foreach (var kvp in dictionary)
            {
                jsonObject[kvp.Key] = JToken.FromObject(kvp.Value, serializer);
            }

            jsonObject.WriteTo(writer);
        }
    }

}
