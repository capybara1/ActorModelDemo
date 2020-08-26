using System;

namespace ActorModelDemo.Akka.Messages
{
    internal sealed class GetHtmlResponse
    {
        public GetHtmlResponse(string html)
        {
            Html = html ?? throw new ArgumentNullException(nameof(html));
        }

        public string Html { get; }
    }
}
