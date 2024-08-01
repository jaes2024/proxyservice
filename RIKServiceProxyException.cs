using System;

namespace LKQC.Windows.Common.ServiceProxy
{
    public class RIKServiceProxyException:Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyException()
        : base()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyException(string message)
        : base(message)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyException(string format, params object[] args)
             : base(string.Format(format, args))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyException(string message, Exception innerException)
        : base(message, innerException)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        { }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //protected RIKServiceProxyException(SerializationInfo info, StreamingContext context)
        //: base(info, context)
        //{ }
    }
}
