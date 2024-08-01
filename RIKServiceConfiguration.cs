using System.Collections.Generic;
using System.Xml.Serialization;

namespace LKQC.Windows.Common.ServiceProxy
{
    [XmlRoot("Definitions")]
    public class RIKServiceConfiguration
    {
        /// <summary>
        /// Service Defination
        /// </summary>
        [XmlArray("Services")]
        [XmlArrayItem("Service")]
        public List<RIKServiceDefinations> ServiceDefination { get; set; }
    }
}
