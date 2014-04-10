using System;
using System.IO;
using System.Xml.Serialization;

namespace HideOut.BmFont
{
    [Serializable]
    public class FontPage
    {
        [XmlAttribute("id")]
        public Int32 ID
        {
            get;
            set;
        }

        [XmlAttribute("file")]
        public String File
        {
            get;
            set;
        }
    }
}
