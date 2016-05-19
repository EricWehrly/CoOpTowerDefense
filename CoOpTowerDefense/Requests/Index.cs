using Nancy;

namespace SoManyZombies.Requests
{
    public class Index : NancyModule
    {
        public Index()
        {
            Get["/"] = BuildIndexPage;
        }

        private object BuildIndexPage(dynamic parameters)
        {
            /*
            return new Response
            {
                StatusCode = HttpStatusCode.OK
            };
            */
            return "<body><script type=\"text\\javascript\">alert('sup');</script>Hi.</body>";
        } 
    }
}
