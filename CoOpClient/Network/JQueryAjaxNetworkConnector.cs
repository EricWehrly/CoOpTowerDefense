using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace CoOpClient.Network
{
    [JsType(JsMode.Clr, Filename = "../res/Network.js")]
    public class JQueryAjaxNetworkConnector : IClientNetworkConnector
    {
        public void SendCommand(string command, object data)
        {
            var ajaxSettings = new AjaxSettings
            {

// TODO: Actually get the proper server hostname URL
#if DEBUG
                url = "http://localhost:1337/" + command,
#else
                url = "http://td.kleisden.com/" + command,
#endif
                // url = "http://localhost:1337/" + command,
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
                url = "http://localhost:1337/" + query,
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