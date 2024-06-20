using MagicT.Shared.Formatters;
using MagicT.Shared.Managers;
using MemoryPack;
using Microsoft.Extensions.DependencyInjection;

namespace MagicT.Shared.Extensions;

public static class DependencyExtensions
{
    public static void RegisterShared(this IServiceCollection services)
    {
        MemoryPackFormatterProvider.Register(new UnsafeObjectFormatter());

        services.AddSingleton(typeof(LogManager<>));


      
    }
}
