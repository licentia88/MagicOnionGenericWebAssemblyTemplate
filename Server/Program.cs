using Coravel;
using MagicOnion.Serialization.MemoryPack;
using MagicT.Server.Database;
using MagicT.Server.Invocables;
using MagicT.Server.Managers;
using EntityFramework.Exceptions.SqlServer;
using LitJWT;
using LitJWT.Algorithms;
using MagicT.Server.Initializers;
using MagicT.Server.Jwt;
using MagicT.Shared.Extensions;
using MagicT.Shared.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddGrpc(x =>
{
    x.EnableDetailedErrors = true;
    x.MaxReceiveMessageSize = null; // 100 MB
    x.MaxSendMessageSize = null; // 100 MB
});

builder.Services.AddMagicOnion(x =>
{
    //-:cnd
#if DEBUG
    x.IsReturnExceptionStackTraceInErrorDetail = true;
#endif
    //+:cnd
    //Remove this line to use magic onion with message pack
    x.MessageSerializer = MemoryPackMagicOnionSerializerProvider.Instance;

});

//builder.Services.RegisterShared("127.0.0.1", 3215);
builder.Services.AutoRegisterFromMagicTShared();
builder.Services.AutoRegisterFromMagicTServer();
builder.Services.RegisterShared();
builder.Services.AddSingleton<TokenManager>();

builder.Services.AddSingleton<AuthenticationManager>();

builder.Services.AddSingleton<AuditManager>();

builder.Services.AddSingleton<QueryManager>();

builder.Services.AddSingleton<FileTransferManager>();

builder.Services.AddScoped<CancellationTokenManager>();

builder.Services.AddQueue();

builder.Services.AddTransient(typeof(AuditFailedInvocable<>));

builder.Services.AddTransient(typeof(AuditRecordsInvocable<>));

builder.Services.AddTransient(typeof(AuditQueryInvocable<>));

builder.Services.AddDbContext<MagicTContext>((_, options) =>
  options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(MagicTContext))!)
  .UseExceptionProcessor()
  .EnableSensitiveDataLogging()
  );

builder.Services.AddDbContext<MagicTInMemoryContext>((_, options) =>
  options.UseInMemoryDatabase(databaseName: "InMemoryDatabase").UseExceptionProcessor()
  .EnableSensitiveDataLogging()
  );


builder.Services.AddSingleton(_ =>
{
    var key = HS256Algorithm.GenerateRandomRecommendedKey();

    var encoder = new JwtEncoder(new HS256Algorithm(key));
    var decoder = new JwtDecoder(encoder.SignAlgorithm);

    return new MagicTTokenService
    {
        Encoder = encoder,
        Decoder = decoder
    };
});

var app = builder.Build();

using var scope = app.Services.CreateAsyncScope();
var keyExchangeManager = app.Services.GetRequiredService<IKeyExchangeManager>();
keyExchangeManager.Initialize();
 
scope.ServiceProvider.GetRequiredService<DataInitializer>().Initialize();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseGrpcWeb();
app.MapMagicOnionService().EnableGrpcWeb();

app.Run();

