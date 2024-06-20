using Benutomo;
using Grpc.Core;
using MagicOnion;
using MagicT.Server.Database;
using MagicT.Server.Jwt;
using MagicT.Server.Models;
using MagicT.Shared.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace MagicT.Server.Managers;

[AutomaticDisposeImpl]
public partial class AuthenticationManager : IDisposable, IAsyncDisposable
{
    [EnableAutomaticDispose]
    private MagicTInMemoryContext MagicTInMemoryContext { get; set; }


    public AuthenticationManager(IServiceProvider provider)
    {
        MagicTInMemoryContext = provider.CreateScope().ServiceProvider.GetRequiredService<MagicTInMemoryContext>();

    }

    /// <summary>
    ///  Validates the roles of the token against the required roles for the service.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="authenticationData"></param>
    /// <exception cref="ReturnStatusException"></exception>
    public void AuthenticateData(int id, EncryptedData<AuthenticationData> authenticationData)
    {
        //var key = Convert.ToString(id);

        var usedTokens = MagicTInMemoryContext.UsedTokens.AsNoTracking().Where(x => x.Id == id);
 
        var expiredToken = usedTokens
            .FirstOrDefault(x =>  x.EncryptedBytes == authenticationData.EncryptedBytes &&
                                                                      x.Nonce == authenticationData.Nonce &&
                                                                      x.Mac == authenticationData.Mac);
        if (expiredToken is not null)
            throw new ReturnStatusException(StatusCode.Unauthenticated, "Expired Token");

        var currentToken = new UsedTokens(authenticationData.EncryptedBytes, authenticationData.Nonce, authenticationData.Mac);
        currentToken.Id = id;
      
        MagicTInMemoryContext.UsedTokens.Add(currentToken);
       
        MagicTInMemoryContext.SaveChanges();

    }

    public void ValidateRoles(MagicTToken token, string endPoint)
    {
        var permission = MagicTInMemoryContext.PERMISSIONS.FirstOrDefault(x => x.PER_PERMISSION_NAME.Equals(endPoint));

        if (permission is null)
            throw new ReturnStatusException(StatusCode.Unauthenticated, "Permission not implemented");

        ////User permission should match either role or permission itself
        if (!token.Roles.Any(x => x == permission.AB_ROWID || x == permission.PER_ROLE_REFNO))
            throw new ReturnStatusException(StatusCode.Unauthenticated, nameof(StatusCode.Unauthenticated));


    }
}
