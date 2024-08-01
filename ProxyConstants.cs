namespace LKQC.Windows.Common.ServiceProxy
{
    /// <summary>
    /// Service proxy constants
    /// </summary>
    public static class ProxyConstants
    {
        /// <summary>
        /// Request type XML
        /// </summary>
        public const string RequestType_XML = "xml";

        /// <summary>
        /// Request type JSON
        /// </summary>
        public const string RequestType_JSON = "json";

        public const string RequestType_HTML = "html";

        /// <summary>
        /// Header Language ID
        /// </summary>
        public const string Header_Key_LanguageId = "LanguageId";

        /// <summary>
        /// Header  TenantId
        /// </summary>
        public const string Header_Key_TenantId = "TenantId";

        /// <summary>
        /// Header  TokenId
        /// </summary>
        public const string Header_Key_TokenId = "TokenId";

        /// <summary>
        /// Header Object
        /// </summary>
        public const string Header_Key_ProxyHeader = "Authorization";

        /// <summary>
        /// Service config path
        /// </summary>
        public const string Config_ServiceConfigPath = "ServiceConfigurationFilePath";

        public const string JsonErrorData = "{'StatusCode':0,'Data':1,'StatusInfo':null}";
        public const string PackageName = "PackageName";
    }
}
