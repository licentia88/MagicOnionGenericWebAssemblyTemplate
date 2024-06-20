using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagicT.Shared.Models;
using MemoryPack;
using Microsoft.EntityFrameworkCore;

namespace MagicT.Server.Models;

[MemoryPackable]
[Index(nameof(Id))]
public partial class UsedTokens
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Pk { get; set; }
    public int Id { get; set; }

    public byte[] EncryptedBytes { get; set; }

    public byte[] Nonce { get; set; }

    public byte[] Mac { get; set; }

    public UsedTokens(byte[] encryptedBytes, byte[] nonce, byte[] mac)
    {
        EncryptedBytes = encryptedBytes;
        Nonce = nonce;
        Mac = mac;
    }

}

//public class UserPermissions
//{
//    public string Key { get; set; }

//    public ICollection<PERMISSIONS> MyProperty { get; set; }
//}
