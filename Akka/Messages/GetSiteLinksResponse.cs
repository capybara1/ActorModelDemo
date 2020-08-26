using System;
using System.Collections.Generic;
using System.Linq;

namespace ActorModelDemo.Akka.Messages
{
    internal sealed class GetSiteLinksResponse
    {
        public GetSiteLinksResponse(IEnumerable<Uri> uris)
        {
            if (uris == null) throw new ArgumentNullException(nameof(uris));

            Uris = uris.ToArray();
        }

        public ICollection<Uri> Uris { get; }
    }

}
