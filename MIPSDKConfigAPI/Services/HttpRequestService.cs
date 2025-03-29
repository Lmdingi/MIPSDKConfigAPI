using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using static VideoOS.Platform.Messaging.MessageId;
using static VideoOS.Platform.Util.ServiceUtil;

namespace MIPSDKConfigAPI.Services
{
    public class HttpRequestService
    {
        // props

        // fields 
        private readonly ServerId _serverId;
        private readonly LoginSettings _loginSettings;

        // constructor
        public HttpRequestService()
        {
            _serverId = EnvironmentManager.Instance.MasterSite.ServerId;
            _loginSettings = LoginSettingsCache.GetLoginSettings(_serverId.ServerHostname);
        }

        // methods
        public IConfigurationService CreateOAuthClientProxy()
        {
            try
            {
                var uri = HttpRequestService.CalculateServiceUrl(_serverId.ServerHostname, _serverId.Serverport);
                var oauthBinding = GetOAuthBinding(_serverId.Serverport == 443);
                string spn = SpnFactory.GetSpn(uri);

                EndpointAddress endpointAddress = new EndpointAddress(uri, EndpointIdentity.CreateSpnIdentity(spn));

                ChannelFactory<IConfigurationService> channel = new ChannelFactory<IConfigurationService>(oauthBinding, endpointAddress);

                EndpointBehaviorService tokenBehavior = new EndpointBehaviorService(_loginSettings);

                channel.Endpoint.EndpointBehaviors.Add(tokenBehavior);

                ConfigureEndpoint(channel.Endpoint);

                IConfigurationService configurationService = channel.CreateChannel();

                return configurationService;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // helpers
        private static Uri CalculateServiceUrl(string hostName, int port)
        {
            UriBuilder uriBuilder = new UriBuilder(port == 443 ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port);

            var baseUri = uriBuilder.Uri;

            return new Uri(baseUri, "ManagementServer/ConfigurationApiServiceOAuth.svc");
        }

        public static Binding GetOAuthBinding(bool isHttps)
        {
            var binding = new BasicHttpBinding();
            binding.Security.Mode = isHttps ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            if (!Debugger.IsAttached)
            {
                // Avoid timeout if debugging calls to server
                binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
                binding.SendTimeout = TimeSpan.FromMinutes(10);
                binding.CloseTimeout = TimeSpan.FromMinutes(10);
            }
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = Encoding.UTF8;
            binding.UseDefaultWebProxy = true;
            binding.AllowCookies = false;

            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;

            return binding;
        }

        public static void ConfigureEndpoint(ServiceEndpoint serviceEndpoint)
        {
            if (serviceEndpoint == null)
            {
                throw new ArgumentNullException(nameof(serviceEndpoint));
            }

            foreach (OperationDescription operationDescription in serviceEndpoint.Contract.Operations)
            {
                IOperationBehavior operationBehavior = operationDescription.Behaviors[typeof(DataContractSerializerOperationBehavior)];
                if (operationBehavior != null)
                {
                    DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operationBehavior as DataContractSerializerOperationBehavior;
                    if (dataContractSerializerOperationBehavior != null)
                    {
                        dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 2147483647;
                    }
                }
            }
        }
    }
}
