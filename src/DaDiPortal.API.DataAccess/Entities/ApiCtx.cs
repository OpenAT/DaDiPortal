using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DaDiPortal.API.DataAccess.Entities;

internal class ApiCtx : DbContext
{
    public ApiCtx(DbContextOptions<ApiCtx> options) : base(options) { }

    internal DbSet<Donation> Donations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new DonationConfiguration().Configure(modelBuilder.Entity<Donation>());
    }
}

internal class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.ToTable("Donations");
        builder.HasKey(x => x.IdDonation);

        builder.Property(x => x.Date);
        builder.Property(x => x.Amount);
    }
}