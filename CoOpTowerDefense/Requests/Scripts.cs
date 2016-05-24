using Nancy;

namespace CoOpTowerDefense.Requests
{
    public class Scripts : NancyModule
    {
        public Scripts()
        {
            Get["/scripts/{filename}"] = FetchScript;
        }

        private Response FetchScript(dynamic parameters)
        {
            string filePath = "scripts/" + parameters.filename;

            return Response.AsFile(filePath, "text/javascript");
        }
    }
}
