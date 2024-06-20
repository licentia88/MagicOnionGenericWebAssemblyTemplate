using MemoryPack;

namespace MagicT.Shared.Models.ServiceModels;

[MemoryPackable]
public sealed partial class AuthenticationData
{
    public byte[] Token { get; set; }

    public string ContactIdentifier { get; set; }

    public AuthenticationData(byte[] token, string contactIdentifier)
    {
        Token = token;
        ContactIdentifier = contactIdentifier;
    }
}