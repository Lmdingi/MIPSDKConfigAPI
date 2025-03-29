using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform.Login;
using static VideoOS.Platform.Util.ServiceUtil;

namespace MIPSDKConfigAPI.Services
{
    public class EndpointBehaviorService : BehaviorExtensionElement, IEndpointBehavior
    {
        // props
        public override Type BehaviorType
        {
            get
            {
                return typeof(AddTokenBehavior);
            }
        }

        // fields
        private readonly LoginSettings _loginSettings;

        // constructor
        public EndpointBehaviorService(LoginSettings loginSettings)
        {
            _loginSettings = loginSettings;
        }        

        // methods
        protected override object CreateBehavior()
        {
            return new EndpointBehaviorService(_loginSettings);
        }

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (_loginSettings != null)
            {
                BearerAuthorizationHeaderInspector bearerAuthorizationHeaderInspector = new BearerAuthorizationHeaderInspector(_loginSettings.IdentityTokenCache);

                clientRuntime.MessageInspectors.Add(bearerAuthorizationHeaderInspector);
            }
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //throw new NotImplementedException();
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }
    }
}
