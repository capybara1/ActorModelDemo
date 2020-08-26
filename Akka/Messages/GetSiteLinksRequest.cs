using System;

namespace ActorModelDemo.Akka.Messages
{
    internal sealed class GetSiteLinksRequest
    {
        public GetSiteLinksRequest(Uri baseUri, int desiredCount)
        {
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            DesiredCount = desiredCount;
        }

        public Uri BaseUri { get; }

        public int DesiredCount { get;}
    }
}
