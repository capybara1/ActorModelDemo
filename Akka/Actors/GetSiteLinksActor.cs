using ActorModelDemo.Akka.Messages;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorModelDemo.Akka.Actors
{
    internal class GetSiteLinksActor : UntypedActor
    {
        private readonly Uri _baseUri;
        private readonly int _desiredCount;
        private readonly HashSet<Uri> _uris = new HashSet<Uri>();

        private IActorRef _downloadActor;
        private IActorRef _getLinksActor;

        public GetSiteLinksActor(Uri baseUri, int desiredCount)
        {
            _baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            _desiredCount = desiredCount;
        }

        public static Props CreateProps(Uri baseUri, int count) => Props.Create(() => new GetSiteLinksActor(baseUri, count));

        protected override void PreStart()
        {
            _downloadActor = Context.ActorOf(DownloadHtmlActor.CreateProps(), "download-actor");
            _getLinksActor = Context.ActorOf(GetPageLinksActor.CreateProps(), "getlinks-actor");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "start":
                    if (_desiredCount > 0) _uris.Add(_baseUri);
                    if (_desiredCount < 2) Context.Parent.Tell(new GetSiteLinksResponse(_uris));
                    _downloadActor.Tell(new GetHtmlRequest(_baseUri));
                    break;

                case GetHtmlResponse res:
                    _getLinksActor.Tell(new GetPageLinksRequest(res.Html));
                    break;

                case GetHtmlError err:
                    Context.Parent.Tell(err);
                    break;

                case GetPageLinksResponse res:
                    if (_uris.Count == _desiredCount) break;
                    var absoluteUris = res.Uris
                        .Select(uri => uri.IsAbsoluteUri ? uri : new Uri(_baseUri, uri));
                    foreach (var uri in absoluteUris)
                    {
                        var notVisited = _uris.Add(uri);
                        if (_uris.Count == _desiredCount)
                        {
                            Context.Parent.Tell(new GetSiteLinksResponse(_uris));
                            break;
                        }
                        if (notVisited)
                        {
                            _downloadActor.Tell(new GetHtmlRequest(uri));
                        }
                    }
                    break;
            }
        }
    }
}
