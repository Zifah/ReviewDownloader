using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewCurator.Utility
{
    public static class HtmlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode">The parent node</param>
        /// <param name="childType">The type of the desired child node</param>
        /// <param name="equals">If true, class must be Equal to supplied value, else Contains will be used</param>
        /// <param name="className">Desired class name</param>
        /// <returns></returns>
        public static HtmlNode GetFirstChildElementWithClass(this HtmlNode parentNode, string className, string childType, bool equals)
        {
            return GetFirstChildWithAttributeAs(parentNode, "class", className, childType, equals);
        }

        public static IEnumerable<HtmlNode> GetAllChildrenWithClassAs(this HtmlNode parentNode, string className, string childType, bool equals)
        {
            return GetAllChildrenWithAttributeAs(parentNode, "class", className, childType, equals);
        }

        public static HtmlNode GetFirstChildWithAttributeAs(this HtmlNode parentNode, string attributeName, string attributeValue, string childType, bool equals)
        {
            return equals ? parentNode.Descendants(childType).FirstOrDefault(x => x.GetAttributeValue(attributeName, "").Equals(attributeValue)) :
            parentNode.Descendants(childType).FirstOrDefault(x => x.GetAttributeValue(attributeName, "").Contains(attributeValue));
        }

        public static IEnumerable<HtmlNode> GetAllChildrenWithAttributeAs(this HtmlNode parentNode, string attributeName, string attributeValue, string childType, bool equals)
        {
            return equals ? parentNode.Descendants(childType).Where(x => x.GetAttributeValue(attributeName, "").Equals(attributeValue)) :
            parentNode.Descendants(childType).Where(x => x.GetAttributeValue(attributeName, "").Contains(attributeValue));
        }

        public static HtmlNode GetDivWithClass(this HtmlNode parentNode, string className, bool equals)
        {
            return parentNode.GetFirstChildElementWithClass(className, "div", equals);
        }

        public static IEnumerable<HtmlNode> GetAllDivsWithClass(this HtmlNode parentNode, string className, bool equals)
        {
            return parentNode.GetAllChildrenWithClassAs(className, "div", equals);
        }

        public static HtmlNode GetSpanWithClass(this HtmlNode parentNode, string className, bool equals)
        {
            return parentNode.GetFirstChildElementWithClass(className, "span", equals);
        }

        public static HtmlNode GetLinkWithClass(this HtmlNode parentNode, string className, bool equals)
        {
            return parentNode.GetFirstChildElementWithClass(className, "a", equals);
        }
    }
}