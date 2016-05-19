using System.Diagnostics;
using System.Threading;
using Nancy;

namespace SoManyZombies.Requests
{
    public class RebuildServer : NancyModule
    {
        public RebuildServer()
        {
            Get["/Rebuild"] = LaunchRebuildScript;
        }

        private object LaunchRebuildScript(dynamic parameters)
        {
            var pr = new Process();
            var prs = new ProcessStartInfo {FileName = @"../../../server-rebuild.sh"};
            pr.StartInfo = prs;

            var ths = new ThreadStart(() => pr.Start());
            new Thread(ths).Start();

            return "Initiating rebuild.";
        } 
    }
}
