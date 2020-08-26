using ActorModelDemo.Akka.Actors;
using ActorModelDemo.Akka.Messages;
using Akka.Actor;
using System;
using System.Threading.Tasks;

namespace ActorModelDemo.Akka
{
    internal class Program
    {
        private const int DefaultOutputSize = 5;

        private static async Task Main(string[] args)
        {
            var baseUri = new Uri(args[0]);
            var outputSize = args.Length > 1
                ? int.Parse(args[1])
                : DefaultOutputSize;


            using var system = ActorSystem.Create("getlink-system");
            var supervisor = system.ActorOf(
                SupervisorActor.CreateProps(),
                "supervisor");


            supervisor.Tell(new GetSiteLinksRequest(baseUri, outputSize));


            Console.WriteLine("Press any key to exit application");
            Console.WriteLine();
            Console.Read();

            await CoordinatedShutdown.Get(system)
                .Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
