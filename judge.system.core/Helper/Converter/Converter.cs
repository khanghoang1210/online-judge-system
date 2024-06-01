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
}
