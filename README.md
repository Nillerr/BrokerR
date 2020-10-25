# BrokerR.Http

A broker that publishes incoming HTTP requests to observers subscribed using SignalR.

## Getting Started

`BrokerR.Http` consists of a server and a client. To connect to an existing `BrokerR.Http.Server`, use:

```c#
var connection = new BrokerRConnectionBuilder(url)
    .WithHandler("/incoming/path", async (request, cancellationToken) => {
        Console.WriteLine($"Incoming Request: {JsonSerializer.Serialize(request)}");
    });

await connection.StartAsync();
```

Sending a request to the server specified by `url` on the path `/incoming/path`, will trigger the handler and 
write `Incoming Request: <json>` to the console. 

A server is provided in `BrokerR.Http.Server`, however the code is rather simple and can easily be copied 
into an existing ASP.NET Core application, if you wish to not have a dedicated `BrokerR.Http.Server` running.

## Purpose

`BrokerR.Http` was developed to enable consuming webhooks from external systems in local development environments.