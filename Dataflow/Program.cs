using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ActorModelDemo.Dataflow
{
    internal class Program
    {
        private const int DefaultOutputSize = 5;

        private static readonly Regex AnchorRef = new Regex(@"(?<=\<a\b[^\>]+\bhref\b\s*=\s*"")[^""]*");

        private static async Task Main(string[] args)
        {
            var baseUri = new Uri(args[0]);
            var outputSize = args.Length > 1
                ? int.Parse(args[1])
                : DefaultOutputSize;


            var downloadString = new TransformBlock<Uri, string>(
                async uri => {
                    using var client = new HttpClient();
                    return await client.GetStringAsync(uri);
                });

            var visitedUris = new HashSet<Uri>();
            var findUris = new TransformManyBlock<string, Uri>(
                html => AnchorRef.Matches(html)
                    .Cast<Match>()
                    .Select(m => new Uri(m.Value, UriKind.RelativeOrAbsolute))
                    .Select(uri => uri.IsAbsoluteUri ? uri : new Uri(baseUri, uri))
                    .Where(uri => visitedUris.Add(uri)),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                });

            var broadcastUris = new BroadcastBlock<Uri>(uri => uri);

            var collectResults = new BatchBlock<Uri>(outputSize);


            downloadString.LinkTo(findUris);

            findUris.LinkTo(broadcastUris);

            broadcastUris.LinkTo(collectResults);

            broadcastUris.LinkTo(
                downloadString,
                uri => baseUri.IsBaseOf(uri) && uri != baseUri);


            downloadString.Post(baseUri);

            try
            {
                var uris = await collectResults.ReceiveAsync(TimeSpan.FromSeconds(5));
                PrintResults(uris);
            }
            catch (InvalidOperationException exception) // time out
            {
                PrintException(exception);
            }
            finally
            {
                downloadString.Complete();
            }

            try
            {
                await downloadString.Completion;
            }
            catch (AggregateException exception)
            {
                PrintException(exception);
            }
            catch (Exception exception)
            {
                PrintException(exception);
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