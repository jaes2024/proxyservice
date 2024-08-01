using System.Collections.Generic;
using System.Xml.Serialization;

namespace LKQC.Windows.Common.ServiceProxy
{
    public class RIKServiceDefinations
    {
        /// <summary>
        /// Service Name
        /// </summary>
        [XmlAttribute("serviceName")]
        public string ServiceName { get; set; }


        /// <summary>
        /// Service Url
        /// </summary>
        [XmlAttribute("serviceUrl")]
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Method Defination
        /// </summary>
        [XmlArray("Methods")]
        [XmlArrayItem("Method")]
        public List<RIKMethodDefination> MethodDefinations { get; set; }
    }
}
