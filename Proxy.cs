using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using LKQC.Windows.Common.Utility;
using Newtonsoft.Json;
using System.IO.Compression;
using Windows.Storage;

namespace LKQC.Windows.Common.ServiceProxy
{
    public class Proxy : IDisposable
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="methodName"></param>
        /// <param name="header"></param>
        public Proxy(string serviceCode, string methodName, ProxyHeader header)
        {
            ServiceCode = serviceCode;
            MethodCode = methodName;
            RequeProxyHeader = header;
        }

        /// <summary>
        /// Service Code
        /// </summary>
        public string ServiceCode { get; set; }
        /// <summary>
        /// MethodCode
        /// </summary>
        public string MethodCode { get; set; }

        private RIKServiceConfiguration _rikServices;
        private RIKServiceConfiguration RikServices => _rikServices ?? (_rikServices = LoadConfiguration());
        
        /// <summary>
        /// Proxy header object
        /// </summary>
        public object RequeProxyHeader { get; set; }

        private RIKServiceConfiguration LoadConfiguration()
        {
            var serializer = new XmlSerializer(typeof(RIKServiceConfiguration));

            var fileName = Constants.ConfigurationFileUrl;
            using (var reader = XmlReader.Create(fileName))
            {
                RIKServiceConfiguration info = (RIKServiceConfiguration)serializer.Deserialize(reader);
                return info;
            }
        }
        
        /// <summary>
        /// Process Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceParams"></param>
        /// <returns>Return T Object</returns>
        public T ProcessRequest<T>(params object[] serviceParams)
        {
            return ProcessServiceRequest<T>(serviceParams);
        }

        public static string MenuName;
        private T ProcessServiceRequest<T>(params object[] serviceParams)
        {
            if (RikServices.ServiceDefination.Any(service => service.ServiceName.ToUpperInvariant() == ServiceCode.ToUpperInvariant()) && RikServices.ServiceDefination.FirstOrDefault(srv => srv.ServiceName.ToUpperInvariant() == ServiceCode.ToUpperInvariant()).MethodDefinations.Any(method => method.Name.ToUpperInvariant() == MethodCode.ToUpperInvariant()))
            {
                RIKServiceDefinations serviceDefination = RikServices.ServiceDefination.FirstOrDefault(srv => srv.ServiceName.ToUpperInvariant() == ServiceCode.ToUpperInvariant());
                RIKMethodDefination methodDef = RikServices.ServiceDefination.FirstOrDefault(srv => srv.ServiceName.ToUpperInvariant() == ServiceCode.ToUpperInvariant()).MethodDefinations.FirstOrDefault(method => method.Name.ToUpperInvariant() == MethodCode.ToUpperInvariant());
                MenuName = methodDef.Name;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(1800);
                        client.DefaultRequestHeaders.Accept.Clear();

                        if (methodDef.RIKFormatter.ToUpperInvariant().Equals(ProxyConstants.RequestType_XML.ToUpperInvariant()))
                        {
                            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                        }
                        else if (
                            methodDef.RIKFormatter.ToUpperInvariant()
                                .Equals(ProxyConstants.RequestType_HTML.ToUpperInvariant()))
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                        }
                        else
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        }

                        if (RequeProxyHeader != null)
                        {
                            var headerValue = JsonConvert.SerializeObject(RequeProxyHeader);
                            client.DefaultRequestHeaders.TryAddWithoutValidation(ProxyConstants.Header_Key_ProxyHeader, headerValue);
                        }


                        string mapping = methodDef.Mapping;
                        string url = serviceDefination.ServiceUrl + "/";

                        if (serviceParams != null && serviceParams.Any())
                        {
                            if (methodDef.Method == "get")
                            {
                                var jsonData = JsonConvert.SerializeObject(serviceParams[0]);
                                mapping = string.Format(mapping, jsonData);
                            }
                            else
                            {
                                mapping = string.Format(mapping, serviceParams);
                            }

                        }

                        url += mapping;
                       
                        Task<HttpResponseMessage> response = null;

                        switch (methodDef.Method)
                        {
                            case "get":
                                using ((IDisposable)(response = client.GetAsync(url)))
                                {
                                    return ProcessResponse<T>(response, methodDef.Name);
                                }

                            case "post":
                                if (methodDef.RIKFormatter == "json" && serviceParams.Any())
                                {
                                    if (serviceParams != null)
                                    {
                                        var jsonData = JsonConvert.SerializeObject(serviceParams[0]);

                                        using ((IDisposable)(response = client.PostAsync(url, string.IsNullOrWhiteSpace(jsonData) ? null : new StringContent(jsonData, Encoding.UTF8, "application/json"))))
                                        {
                                            return ProcessResponse<T>(response, methodDef.Name);
                                        }
                                    }
                                }
                                if (methodDef.RIKFormatter == "xml" && serviceParams.Any())
                                {
                                    if (serviceParams != null)
                                    {
                                        var xmlData = Convert.ToString(serviceParams[0]);
                                        using ((IDisposable)(response = client.PostAsync(url, string.IsNullOrWhiteSpace(xmlData) ? null : new StringContent(xmlData, Encoding.UTF8, "text/plain"))))
                                        {
                                            return ProcessResponse<T>(response, methodDef.Name);
                                        }
                                    }
                                }
                                if (methodDef.RIKFormatter == "html" && serviceParams.Any())
                                {
                                    if (serviceParams != null)
                                    {
                                        var htmlData = Convert.ToString(serviceParams[0]);
                                        using ((IDisposable)(response = client.PostAsync(url, string.IsNullOrWhiteSpace(htmlData) ? null : new StringContent(htmlData, Encoding.UTF8, "text/html"))))
                                        {
                                            return ProcessResponse<T>(response, methodDef.Name);
                                        }
                                    }
                                }
                                break;
                            default:
                                throw new RIKServiceProxyException("Invalid method");


                        }
                        return ProcessResponse<T>(null, methodDef.Name);
                    }
                }
                catch (Exception ex)
                {
                    throw new RIKServiceProxyException(ex.Message);
                }
            }
           
            return ProcessErrorResponse<T>();
        }

        private static TResult ProcessResponse<TResult>(Task<HttpResponseMessage> responseMessage,string methodName)
        {
            var result = responseMessage.Result;

            switch (result.StatusCode)
            {
                case HttpStatusCode.Created:
                case HttpStatusCode.OK:
                    {
                        return UnzipFile<TResult>(result.Content.ReadAsByteArrayAsync().Result);
                        //return Deserialize<TResult>(result.Content.ReadAsStringAsync().Result);
                    }
                case HttpStatusCode.NotFound:
                    throw new RIKServiceProxyException("Bad Request : Request URL not found.");
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                    throw new RIKServiceProxyException("Access Denied : Token has been expired.");
                  
                case HttpStatusCode.InternalServerError:
                    throw new RIKServiceProxyException("Internal Server Error :"+ methodName);
                default:
                    throw new RIKServiceProxyException("Unknown Response");
            }

        }


        private static TResult ProcessErrorResponse<TResult>()
        {
            string jsonError = ProxyConstants.JsonErrorData;
            return Deserialize<TResult>(jsonError);
        }

        /// <summary>
        /// Deserialize the response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static T DeserializeResponse<T>(string response)
        {
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deserialize the response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The response.</param>
        /// <returns></returns>
        /// <exception cref="HDServiceProxyException"></exception>
        public static T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new RIKServiceProxyDeserializationException(ex.Message, ex.InnerException);
            }
        }

        private static T UnzipFile<T>(byte[] json)
        {
            try
            {
                using (var compressedStream = new MemoryStream(json))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    var result = resultStream.ToArray();
                    string result1 = Encoding.UTF8.GetString(result);
                    return JsonConvert.DeserializeObject<T>(result1);
                }
            }
            catch (Exception ex)
            {
                throw new RIKServiceProxyDeserializationException(ex.Message, ex.InnerException);
            }
        }
        
        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _rikServices = null;
            }
        }
    }
}
