using ActorModelDemo.Akka.Messages;
using Akka.Actor;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ActorModelDemo.Akka.Actors
{
    internal class GetPageLinksActor : UntypedActor
    {
        private static readonly Regex AnchorRef = new Regex(@"(?<=\<a\b[^\>]+\bhref\b\s*=\s*"")[^""]*");

        public static Props CreateProps() => Props.Create<GetPageLinksActor>();

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetPageLinksRequest req:
                    var uris = AnchorRef.Matches(req.Html)
                        .Cast<Match>()
                        .Select(m => new Uri(m.Value, UriKind.RelativeOrAbsolute));
                    Sender.Tell(new GetPageLinksResponse(uris));
                    break;
            }
        }
    }
}
