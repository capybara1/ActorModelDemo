using System;

namespace ActorModelDemo.Akka.Messages
{
    internal sealed class GetHtmlError
    {
        public GetHtmlError(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public Exception Exception { get; }
    }
}
