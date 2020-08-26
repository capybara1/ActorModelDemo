using ActorModelDemo.Akka.Messages;
using Akka.Actor;
using System;
using System.Collections.Generic;

namespace ActorModelDemo.Akka.Actors
{
    internal class SupervisorActor : UntypedActor
    {
        public static Props CreateProps() => Props.Create<SupervisorActor>();

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetSiteLinksRequest req:
                    var actor = Context.ActorOf(GetSiteLinksActor.CreateProps(req.BaseUri, req.DesiredCount));
                    actor.Tell("start");
                    break;

                case GetHtmlError err:
                    PrintException(err.Exception);
                    break;

                case GetSiteLinksResponse res:
                    PrintResults(res.Uris);
                    break;

                case "stop":
                    Context.Stop(Self);
                    break;
            }
        }

        private static void PrintResults(IEnumerable<Uri> uris)
        {
            Console.WriteLine("Results:");
            foreach (var uri in uris)
            {
                Console.WriteLine("- {0}", uri);
            }
            Console.WriteLine();
        }

        private static void PrintException(AggregateException exception)
        {
            exception.Handle(inner => {
                PrintException(inner);
                return true;
            });
        }

        private static void PrintException(Exception exception)
        {
            if (exception is AggregateException aggregateException)
            {
                PrintException(aggregateException);
                return;
            }

            Console.WriteLine(
                "Encountered {0}: {1}",
                exception.GetType().Name,
                exception.Message);
            Console.WriteLine();
        }
    }
}
