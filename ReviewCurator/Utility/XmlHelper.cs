using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ReviewCurator.Utility
{
    public class XmlHelper
    {
        /// <summary>
        /// Convert C# object to its equivalent XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToSerialize"></param>
        /// <returns></returns>
        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings()
                    {
                        // If set to true XmlWriter would close MemoryStream automatically and using would then do double dispose
                        // Code analysis does not understand that. That's why there is a suppress message.
                        CloseOutput = false,
                        Encoding = new UTF8Encoding(false), //UTF8 without BOM (false)
                        OmitXmlDeclaration = false,
                        Indent = true
                    };

                    using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(ms, xmlWriterSettings))
                    {
                        XmlSerializer s = new XmlSerializer(typeof(T));
                        s.Serialize(xw, dataToSerialize);
                    }

                    var result = Encoding.UTF8.GetString(ms.ToArray());
                    result = RemoveWhiteSpaceBetweenXmlTags(result);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Convert XML string to its equivalent C# object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }

        private static string RemoveWhiteSpaceBetweenXmlTags(string xmlString)
        {
            var cleanXml = Regex.Replace(xmlString, @">\s*<", "><");
            return cleanXml;
        }
    }
}
