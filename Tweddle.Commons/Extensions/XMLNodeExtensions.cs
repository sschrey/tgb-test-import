using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Tweddle.Commons.Extensions
{
    public static class XmlNodeExtensions
    {
        public static XmlElement AddElement(this XmlNode node, string name)
        {
            return (XmlElement)node.AppendChild(node.OwnerDocument.CreateElement(name));
        }
        
    }
}
