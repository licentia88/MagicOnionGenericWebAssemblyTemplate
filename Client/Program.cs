

using System;
using System.Collections.Generic;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using MagicOnion.Client;
using MagicOnion.Serialization.MemoryPack;
using MagicT.Client;
using MagicT.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Generator.Components.Extensions;
using MudBlazor.Services;
using MagicT.Client.Models;
using Blazored.LocalStorage;
using MagicT.Client.Seed;
using MagicT.Shared.Extensions;
using MagicT.Client.Services;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.RegisterGeneratorComponents();
builder.Services.AutoRegisterFromMagicTClient();
builder.Services.AutoRegisterFromMagicTShared();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<NotificationVM>();
builder.Services.AddMudServices();

builder.Services.AddHttpContextAccessor();
builder.Services.RegisterShared();

builder.Services.AddSingleton(typeof(List<>));

builder.Services.AddSingleton(typeof(Lazy<>));

builder.Services.AddMessagePipe();
//builder.Services.AddHttpClient("MagicT.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

//// Supply HttpClient instances that include access tokens when making requests to the server project
//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MagicT.ServerAPI"));


builder.Services.AddSingleton(x => GrpcChannel.ForAddress(builder.HostEnvironment.BaseAddress, new GrpcChannelOptions
{
    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())

}));

 

var app =   builder.Build();

await using var scope = app.Services.CreateAsyncScope();

if (scope.ServiceProvider.GetService<IKeyExchangeService>() is KeyExchangeService keyExchangeService) 
     await keyExchangeService.GlobalKeyExchangeAsync();

var seedDataService = app.Services.GetService<SeedData>();
await seedDataService.InitializeAsync();
await app.RunAsync();

