using System;
using AQueryMaker.Extensions;
using MagicOnion;
using MagicT.Server.Database;
using MagicT.Server.Services.Base;
using MagicT.Shared.Models;
using MagicT.Shared.Services;

//using MapDataReader;

namespace MagicT.Server.Services;

 
public class AuditQueryService : MagicServerSecureService<IAuditQueryService, AUDIT_QUERY,MagicTContext>, IAuditQueryService
{
    public AuditQueryService(IServiceProvider provider) : base(provider)
    {
    }

    

}