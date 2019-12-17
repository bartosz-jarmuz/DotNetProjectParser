using System;
using System.Xml.Linq;

namespace DotNetProjectParser.Readers
{
    /// <summary>
    /// Parses the condition expression
    /// </summary>
    public static class ConditionParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionAttribute"></param>
        public static Condition Parse(XAttribute conditionAttribute)
        {
            if (conditionAttribute == null) throw new ArgumentNullException(nameof(conditionAttribute));

            var condition = new Condition();
            condition.Expression = conditionAttribute.Value.Trim();
            if (condition.Expression.StartsWith("'$(Configuration)|$(Platform)'",
                StringComparison.OrdinalIgnoreCase))
            {
                var value = condition.Expression.Replace("$(Configuration)|$(Platform)'", "");
                value = value.Replace("==", "");
                value = value.Trim('\'', ' ');
                var parts = value.Split('|');
                condition.Configuration = parts[0];
                condition.Platform = ParsePlatform(parts[1]);
            }

            return condition;
        }

        /// <summary>
        /// Parses the platform value
        /// </summary>
        /// <param name="plaform"></param>
        /// <returns></returns>
        public  static Platform ParsePlatform(string plaform)
        {
            if (Enum.TryParse(plaform, true, out Platform parsed))
            {
                return parsed;
            }

            return Platform.Unspecified;
        }
    }
}