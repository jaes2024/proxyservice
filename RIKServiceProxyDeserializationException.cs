using System;

namespace LKQC.Windows.Common.ServiceProxy
{
    public class RIKServiceProxyDeserializationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyDeserializationException()
        : base()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyDeserializationException(string message)
        : base(message)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyDeserializationException(string format, params object[] args)
             : base(string.Format(format, args))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyDeserializationException(string message, Exception innerException)
        : base(message, innerException)
        { }


        /// <summary>
        /// Constructor
        /// </summary>
        public RIKServiceProxyDeserializationException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        { }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ////protected RIKServiceProxyDeserializationException(SerializationInfo info, StreamingContext context)
        ////: base(info, context)
        ////{ }
    }
}
