using System;

namespace LKQC.Windows.Common.ServiceProxy
{
    public class RIKResultModel<T>
    {
        /// <summary>
        /// Status code 
        /// </summary>
        public Int64? StatusCode { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// StatusInfo
        /// </summary>
        public RIKStatusInfo StatusInfo { get; set; }
    }

    /// <summary>
    /// HDStatusInfo
    /// </summary>
    public class RIKStatusInfo
    {
        /// <summary>
        /// ErrorCode
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
    }
}
