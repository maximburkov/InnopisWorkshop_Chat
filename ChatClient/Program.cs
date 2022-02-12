using ChatClient;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatService;
using Grpc.Net.Client.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddGrpcClient<Messenger.MessengerClient>(options =>
{
    options.Address = new Uri("https://localhost:7058");
}).ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(new HttpClientHandler()));

//builder.Services.AddSingleton(sp =>
//{
//    var channel = GrpcChannel.ForAddress("https://localhost:7058", new GrpcChannelOptions
//    {
//        HttpHandler = new GrpcWebHandler(new HttpClientHandler())
//    });
//    return new Messenger.MessengerClient(channel);
//});

await builder.Build().RunAsync();
