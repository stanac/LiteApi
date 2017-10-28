using System.Collections.Generic;

namespace LiteApi.OpenApi.Models
{
    public static class NameIncrement
    {
        /// <summary>
        /// Increments the name. Returns name increment, as in:
        /// para   --&gt; param1
        /// para1  --&gt; param2
        /// para9  --&gt; param10
        /// para99 --&gt; param100
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string IncrementName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            List<char> numbers = new List<char>();
            for (int i = name.Length - 1; i >= 0; i--)
            {
                if (char.IsNumber(name[i])) numbers.Insert(0, name[i]);
                else break;
            }
            string value = new string(numbers.ToArray());
            if (value.Length == 0) return name + "1";
            int number = int.Parse(value) + 1;
            return name.Substring(0, name.Length - value.Length) + number;
        }
    }
}
