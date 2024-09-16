using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CF.Insurance.Infrastructure.DbContext;

public class InsuranceContext(DbContextOptions<InsuranceContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Domain.Entities.Insurance> Insurances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        InsuranceModelBuilder(modelBuilder);
    }

    private static void InsuranceModelBuilder(ModelBuilder modelBuilder)
    {
        var model = modelBuilder.Entity<Domain.Entities.Insurance>();

        model.ToTable("Insurance");

        model.Property(x => x.CommercialPremium)
            .IsRequired();

        model.HasIndex(x => x.Id)
            .IsUnique();

        model.Property(x => x.InsuranceValue)
            .IsRequired();

        model.Property(x => x.PurePremium)
            .IsRequired();

        model.Property(x => x.RiskPremium)
            .IsRequired();

        model.Property(x => x.Created)
            .IsRequired();

        model.Property(x => x.VehicleId)
            .IsRequired();

        model.Property(x => x.RiskRate)
            .IsRequired();
    }
}