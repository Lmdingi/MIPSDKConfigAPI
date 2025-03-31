using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using static VideoOS.Platform.Util.ServiceUtil;

namespace MIPSDKConfigAPI.Services
{
    public class ConfigApiService
    {
        // props
        public bool Connected { get; set; } = false;
        public IConfigurationService Client { get; set; }

        // fields
        private readonly HttpRequestService _httpRequestService;
        private Dictionary<string, string> _translations = new Dictionary<string, string>();
        private readonly Dictionary<String, MethodInfo> _allMethodInfos = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<String, Bitmap> _allMethodBitmaps = new Dictionary<string, Bitmap>();

        // constructor
        public ConfigApiService()
        {
            _httpRequestService = new HttpRequestService();
            Initialize();
        }

        // methods
        public ConfigurationItem GetItem(string path)
        {
            try
            {
                var item = Client.GetItem(path);
                return item;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        internal ConfigurationItem[] GetChildItems(string path)
        {
            try
            {
                return Client.GetChildItems(path);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // helpers
        private void Initialize()
        {
            try
            {
                Client = _httpRequestService.CreateOAuthClientProxy();

                _translations = Client.GetTranslations("en-US");

                MethodInfo[] methods = Client.GetMethodInfos();

                foreach (MethodInfo mi in methods)
                {
                    _allMethodInfos.Add(mi.MethodId, mi);
                    _allMethodBitmaps.Add(mi.MethodId, MakeBitmap(mi.Bitmap));

                    String translated = _translations.ContainsKey(mi.TranslationId) ? _translations[mi.TranslationId] : "not found";
                }

                Connected = true;
            }
            catch (Exception ex)
            {
                // log
            }
        }
        
        private Bitmap MakeBitmap(byte[] data)
        {
            try
            {
                if (data == null)
                    return null;
                MemoryStream ms = new MemoryStream(data);
                Bitmap b = new Bitmap(ms);
                ms.Close();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
