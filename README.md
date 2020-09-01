# Actor Model Demo

Demonstrates the actor model for .NET using TPL Dataflow and Akka.NET.

## Fundamentals

Motivation:

- First described in 1973 by Carl Hewitt, Peter Bishop und Richard Steiger
- Thread blocking and resource locking are considered computationally expensive
  and introduce the risk of deadlocks
- Parallelism
  - Muliple cores is the norm as of today
- Reducing impacts of locality
  - Processes can run anywhere
  - Immutability implies that copies of the data resides at every location
- Solving problems by allowing processes to crash

Properties of an actor:

- Actors may be stateful
- Actors communicate via immutable messages
  - Need for thread blocking or resource locking
- The location of an actor is transparent 
  - Same CLR, other Node, etc.

Remarks:

- Actors are a first class language component in other
  programming languages such as e.g. Erlang (1987), Go (2009), etc.
  - Erlang also facilitates the concept of supervision
- Imutability is a core principle of functional programming languages

Resources:

- [Actor Model - Wikipedia](https://en.wikipedia.org/wiki/Actor_model)
- [Erlang](https://www.erlang.org/)
- [Erlang Programming Language - Computerphile](https://www.youtube.com/watch?v=SOqQVoVai6s)
- [Immutability - Computerphile](https://www.youtube.com/watch?v=8Sf6ToPNiA4)
- [Go](https://golang.org/)
- [Go (programming language) - Wikipedia](https://en.wikipedia.org/wiki/Go_(programming_language))

## TPL Dataflow Demo

Concepts to cover:

- Coarse-grained vs. fine-grained parallelism
  - Use [PLINQ](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/introduction-to-plinq)
    for _fine-grained_ parallelism
- Greedy vs. non-greedy
- Completion
  - Potential risk: resource starvation
- Dataflow pipelines/networks
  - API
  - Predefined Dataflow block types

Basic Demo:

1.  Start `ActorModelDemo.TestApp`
2.  Run `ActorModelDemo.Dataflow` using the test app's base URI as first argument

Remarks:

- If the desired count is > 5 then resource starvation should (purposefully) occur

Resources:

- [Dataflow (Task Parallel Library)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library)
- [TPL Dataflow in .Net Core, in Depth – Part 1](https://www.blinkingcaret.com/2019/05/15/tpl-dataflow-in-net-core-in-depth-part-1/)
- [TPL Dataflow in .Net Core, in Depth – Part 2](https://www.blinkingcaret.com/2019/06/05/tpl-dataflow-in-net-core-in-depth-part-2/)
- [Potential Pitfalls in Data and Task Parallelism](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/potential-pitfalls-in-data-and-task-parallelism)

## Akka.NET Demo

Concepts to cover:

- Lifecycle
  - Props
- Hierarchy
  - Supervision
  - Coordinated shutdown
- Extensions (not covered by demo)
  - Remote
    - Cluster
  - Persistance
  - Streaming
  - Logging
  - IoC
- Design aspects
  - Immutable message types
  - No async/await in actors
  - Deadlettering

Basic Demo:

1.  Start `ActorModelDemo.TestApp`
2.  Run `ActorModelDemo.Akka` using the test app's base URI as first argument

Remarks:

- If the desired count is > 5 then resource starvation should (purposefully) occur

Resources:

- Origin
  - [Akka](https://akka.io/)
  - [Akka (toolkit)](https://en.wikipedia.org/wiki/Akka_(toolkit))
- Project
  - [Akka.NET](https://getakka.net/) project site
  - [akkadotnet/akka.net](https://github.com/akkadotnet/akka.net) GitHub project
- Articles
  - [The Top 7 Mistakes Newbies Make with Akka.NET](https://petabridge.com/blog/top-7-akkadotnet-stumbling-blocks/)
  - [How to Do Asynchronous I/O with Akka.NET Actors Using PipeTo](https://petabridge.com/blog/akkadotnet-async-actors-using-pipeto/)
  - [Coordinated Shutdown](https://getakka.net/articles/actors/coordinated-shutdown.html)
- Tutorials
  - [Akka.NET Bootcamp](https://github.com/petabridge/akka-bootcamp)
  - [A Look At Akka.NET](https://www.codeproject.com/Articles/1007161/A-Look-At-Akka-NET)
