using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DaDiPortal.IdentityServer.Data;

public class AspNetIdentityDbContext : IdentityDbContext
{
    public AspNetIdentityDbContext(DbContextOptions<AspNetIdentityDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(DbConstants.SchemaAspNetIdentity);
        base.OnModelCreating(builder);
    }
}
