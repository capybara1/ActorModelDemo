using ActorModelDemo.Akka.Messages;
using Akka.Actor;
using System.Net.Http;
using System.Threading.Tasks;

namespace ActorModelDemo.Akka.Actors
{
    internal class DownloadHtmlActor : UntypedActor
    {
        public static Props CreateProps() => Props.Create<DownloadHtmlActor>();

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetHtmlRequest req:
                    var client = new HttpClient();
                    client.GetStringAsync(req.Uri)
                        .ContinueWith<object>(
                            antecedent => {
                                if (antecedent.IsFaulted)
                                    return new GetHtmlError(antecedent.Exception);
                                return new GetHtmlResponse(antecedent.Result);
                            },
                            TaskContinuationOptions.ExecuteSynchronously) // intend: performance optimization
                        .PipeTo(Sender);
                    break;
            }
        }
    }
}
