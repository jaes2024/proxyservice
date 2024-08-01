using System;
using Windows.ApplicationModel;
using Windows.Storage;
using LKQC.Windows.Common.ServiceProxy;
using LKQC.Windows.Common.Utility;

namespace LKQC.Windows.Business.Repository
{
    /// <summary>
    /// BaseServiceProvider
    /// </summary>
    public class BaseServiceProvider : IDisposable
    {
        /// <summary>
        /// ServiceName
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// MethodName
        /// </summary>
        public string MethodName { get; set; }

        private Proxy _rikServiceProxy { get; set; }

        /// <summary>
        /// Service Proxy Object
        /// </summary>
        protected Proxy RIKServiceProxy
        {
            get
            {
                if (_rikServiceProxy == null)
                {
                    RIKServiceProxy = new Proxy("", "", GetProxyHeader());
                }
                return _rikServiceProxy;
            }
            set { _rikServiceProxy = value; }
        }


        protected static BalSynchronize BalSynchronizeRepository { get; set; }
        /// <summary>
        /// constructor 
        /// </summary>
        public BaseServiceProvider()
        {
            RIKServiceProxy = new Proxy("", "",GetProxyHeader());
           
            BalSynchronizeRepository=new BalSynchronize();
           // Localsettings = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// constructor 
        /// </summary>
        public BaseServiceProvider(short searviceCode)
        {
            ServiceName = LkqcHelper.GetServiceName(searviceCode);
            RIKServiceProxy = new Proxy(ServiceName, "", GetProxyHeader());
        }

      //  public static ApplicationDataContainer Localsettings;
        /// <summary>
        /// constructor 
        /// </summary>
        public BaseServiceProvider(short searviceCode, string methodName)
        {
            ServiceName = LkqcHelper.GetServiceName(searviceCode);
            MethodName = methodName;
            RIKServiceProxy = new Proxy(ServiceName, methodName, GetProxyHeader());
            
        }

        /// <summary>
        /// Dispose the class varaibles and objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (RIKServiceProxy != null)
                {
                    RIKServiceProxy.Dispose();
                    RIKServiceProxy = null;
                }
            }
        }

        private static ProxyHeader GetProxyHeader()
        {
            return new ProxyHeader {  TokenId = TokenId, UserId = UserId, RoleId = RoleId, AppId = AppId };
        }

        /// <summary>
        /// TokenId
        /// </summary>
        public static string TokenId
        {
            get
            {
                using (var balCommonRepository = new BalCommonRepository())
                {
                    string existingToken = balCommonRepository.GetRikToken();
                    if (!string.IsNullOrEmpty(existingToken))
                    {
                        return existingToken;
                    }
                    return Package.Current.Id.Name;
                }
            }
        }

        

        /// <summary>
        /// LanguageId
        /// </summary>
        public static string RoleId => "1";

        /// <summary>
        /// UserId
        /// </summary>
        public static string UserId => "1";

        /// <summary>
        /// AppId
        /// </summary>
        public static string AppId => Convert.ToString((int)Enums.ApplicationType.UniversalWindowsApplication);
    }
}
