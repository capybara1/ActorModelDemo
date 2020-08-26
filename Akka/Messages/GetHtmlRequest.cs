using System;

namespace ActorModelDemo.Akka.Messages
{
    internal sealed class GetHtmlRequest
    {
        public GetHtmlRequest(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public Uri Uri { get; }
    }
}
