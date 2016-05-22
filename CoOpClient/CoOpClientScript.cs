using CoOpClient.Network;
using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace CoOpClient
{
    [JsType(JsMode.Global, Filename = "res/CoOpClient.js")]
    public class CoOpClientScript
    {
        private static IClientNetworkConnector _networkConnector;

		private static void SetUpCharacterActions()
		{
			// Declared character actions should be shared between server and client automatically by declaration
			// Maybe declare them in the lib instead
			//var moveRight = new CharacterAction();
		}

        public static void DefaultClient_Load()
        {
            _networkConnector = new JQueryAjaxNetworkConnector();

            new jQuery(HtmlContext.document.body).append("Ready<br/>");

            new jQuery(HtmlContext.document.body).keydown(DocumentKeyDown);

            new jQuery(HtmlContext.document.body).keyup(DocumentKeyUp);

            new jQuery(HtmlContext.document.body).mousedown(DocumentMouseDown);

            new jQuery(HtmlContext.document.body).mousedown(DocumentMouseUp);

            // Notify server of local player join
            ConnectToServer();

            // On server response, create local player assets
            // CreateCharacter();

            // Load remote player data

            HeartBeat.Initialize();

            ClientRenderer.Initialize();
        }

        public static void ConnectToServer()
        {
            _networkConnector.SendQuery("InitialConnect", null);
        }

        private static void DocumentKeyDown(Event arg)
        {
            PlayerInput.ProcessPlayerKeyboardInput(arg.which, true);
        }

        private static void DocumentKeyUp(Event arg)
        {
            PlayerInput.ProcessPlayerKeyboardInput(arg.which, false);
        }

        public static void DocumentMouseDown(Event arg)
        {
            console.log("Mouse button " + arg.button + " down at " + arg.clientX + ", " + arg.clientY);

            PlayerInput.ProcessPlayerMouseInput(arg.keyCode, arg.clientX, arg.clientY, true);
        }

        public static void DocumentMouseUp(Event arg)
        {
            console.log("Mouse button " + arg.button + " up at " + arg.clientX + ", " + arg.clientY);

            PlayerInput.ProcessPlayerMouseInput(arg.keyCode, arg.clientX, arg.clientY, false);
        }
    }

    // For some reason SharpKit requires we do this to write to the console normally.
    // [JsType(JsMode.Prototype, Name = "console")]
    public class console
    {
        public static void log(string message) { }
    }
}