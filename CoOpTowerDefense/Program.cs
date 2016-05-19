using System;

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
            const string nancyUri = "http://localhost:1337/";

            /*
            using (var host = new NancyHost(new Uri(nancyUri)))
            {
                host.Start();
#if DEBUG
                Process.Start(nancyUri);
#endif

                Console.ReadLine();

                host.Stop();

                _shutDownIssued = true;
            }
            */
            Console.ReadLine();
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
