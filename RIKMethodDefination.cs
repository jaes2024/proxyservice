using System.Xml.Serialization;

namespace LKQC.Windows.Common.ServiceProxy
{
    public class RIKMethodDefination
    {
        /// <summary>
        /// Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Method Name
        /// </summary>
        [XmlAttribute("method")]
        public string Method { get; set; }

        /// <summary>
        /// Mapping
        /// </summary>
        [XmlAttribute("mapping")]
        public string Mapping { get; set; }

        /// <summary>
        /// HD Formatter
        /// </summary>
        [XmlAttribute("type")]
        public string RIKFormatter { get; set; }
    }
}
