using System;

namespace ActorModelDemo.Akka.Messages
{
    internal sealed class GetPageLinksRequest
    {
        public GetPageLinksRequest(string html)
        {
            Html = html ?? throw new ArgumentNullException(nameof(html));
        }

        public string Html { get; }
    }
}
