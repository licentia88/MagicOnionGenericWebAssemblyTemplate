using MagicT.Server.Models;
using MagicT.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicT.Server.Database;

public class MagicTInMemoryContext : DbContext
{
    public MagicTInMemoryContext(DbContextOptions<MagicTInMemoryContext> options) : base(options)
    {
    }

    public virtual DbSet<UsersCredentials> UsersCredentials { get; set; }

    public virtual DbSet<UsedTokens> UsedTokens { get; set; }

    public virtual DbSet<PERMISSIONS> PERMISSIONS { get; set; }

    public virtual DbSet<ROLES> ROLES { get; set; }

    public virtual DbSet<ROLES_D> ROLES_D { get; set; }

 }
