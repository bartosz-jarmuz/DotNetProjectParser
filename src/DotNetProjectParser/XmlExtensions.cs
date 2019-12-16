using System;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DotNetProjectParser
{
    internal static class XmlExtensions
    {
        public static XElement GetByLocalName(this XElement element, string localName)
        {
            return element?.Elements().FirstOrDefault(x => x.Name.LocalName == localName);
        }

        public static string GetValueByXpath(this XDocument document, string xpath, XmlNamespaceManager namespaceManager)
        {
            XElement targetElement = document.XPathSelectElements(xpath, namespaceManager).FirstOrDefault();
            return targetElement?.Value;
        }

        public static T GetValueByLocalName<T>(this XElement element, string localName)
        {
            var target =  element?.Elements().FirstOrDefault(x => x.Name.LocalName == localName);
            return GetValue<T>(target);
        }

        public static T GetValue<T>(this XElement element)
        {
            string stringifiedValue = element?.Value;
            if (string.IsNullOrEmpty(stringifiedValue))
            {
                return default(T);
            }
            try
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(stringifiedValue);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Failed to convert value [{stringifiedValue}] of element [{element}] as {typeof(T).Name}.", ex);
            }
        }

        public static T GetValueByXpath<T>(this XDocument document, string xpath, XmlNamespaceManager namespaceManager)
        {
            XElement targetElement = document.XPathSelectElements(xpath, namespaceManager).FirstOrDefault();
            string stringifiedValue = targetElement?.Value;
            if (string.IsNullOrEmpty(stringifiedValue))
            {
                return default(T);
            }
            try
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(stringifiedValue);
            }
            catch (Exception ex)
            {
               throw new FormatException($"Failed to convert value [{stringifiedValue}] of xpath [{xpath}] as {typeof(T).Name}.", ex);
            }
        }


    }
}