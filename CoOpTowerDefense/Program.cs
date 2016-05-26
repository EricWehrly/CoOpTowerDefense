using System;
using System.Diagnostics;
using System.Threading;
using Nancy.Hosting.Self;

namespace CoOpTowerDefense
{
    class Program
    {
        private static bool _shutDownIssued = false;

        static void Main(string[] args)
        {
            StartGameServer();
        }

        private static void StartGameServer()
        {
#if DEBUG
            const string nancyUri = "http://localhost:1337/";
#else
            const string nancyUri = "http://localhost:80/";
#endif

            using (var host = new NancyHost(new Uri(nancyUri)))
            {
                
                host.Start();
#if DEBUG
                Process.Start(nancyUri);
#endif
                while (!_shutDownIssued)
                {
                    Thread.Sleep(60000);
                }
                //Console.ReadLine();
                host.Stop();

                _shutDownIssued = true;
            }
        }

        private static void StartGameLoop()
        {
            while (!_shutDownIssued)
            {
                // PhysicsEngine.MainLoop();
            }
        }
    }
}
