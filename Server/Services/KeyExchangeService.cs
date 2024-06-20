using Benutomo;
using MagicOnion;
using MagicT.Server.Database;
using MagicT.Server.Managers;
using MagicT.Server.Services.Base;
using MagicT.Shared.Managers;
using MagicT.Shared.Services;

namespace MagicT.Server.Services;

[AutomaticDisposeImpl]
public sealed partial class KeyExchangeService : MagicServerService<IKeyExchangeService, byte[],MagicTContext>, IKeyExchangeService, IDisposable,IAsyncDisposable
{
    public IKeyExchangeManager KeyExchangeManager { get; set; }


    public KeyExchangeService(IServiceProvider provider) : base(provider)
    {
        KeyExchangeManager = provider.GetRequiredService<IKeyExchangeManager>();
    }


    public UnaryResult<byte[]> GlobalKeyExchangeAsync(byte[] clientPublic)
    {
        KeyExchangeManager.KeyExchangeData.SharedBytes = KeyExchangeManager.CreateSharedKey(clientPublic, KeyExchangeManager.KeyExchangeData.PrivateKey);

        return new UnaryResult<byte[]>(KeyExchangeManager.KeyExchangeData.SelfPublicBytes);
    }
 
}