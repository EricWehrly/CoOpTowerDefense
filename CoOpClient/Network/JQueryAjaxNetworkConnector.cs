using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace CoOpClient.Network
{
    [JsType(JsMode.Clr, Filename = "../res/Network.js")]
    public class JQueryAjaxNetworkConnector : IClientNetworkConnector
    {
        // TODO: Actually get the proper server hostname URL
#if DEBUG
        private const string serverAddress = "http://localhost:1337/";
#else
        private const string serverAddress = "http://td.kleisden.com/";
#endif

        public void SendCommand(string command, object data)
        {
            var ajaxSettings = new AjaxSettings
            {
                url = serverAddress + command,
                cache = false,
                data = data,
                dataType = "",
                // success = Success
            };

            jQuery.ajax(ajaxSettings);
        }

        public object SendQuery(string query, object data)
        {
            var ajaxSettings = new AjaxSettings
            {
                url = serverAddress + query,
                cache = false,
                data = data,
                dataType = "",
                // success = Success
            };

            jQuery.ajax(ajaxSettings);

            return "";
        }
    }
}