using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CF.Vehicle.Infrastructure.DbContext;

public class VehicleContext(DbContextOptions<VehicleContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Domain.Entities.Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        VehicleModelBuilder(modelBuilder);
    }

    private static void VehicleModelBuilder(ModelBuilder modelBuilder)
    {
        var model = modelBuilder.Entity<Domain.Entities.Vehicle>();

        model.ToTable("Vehicle");

        model.Property(x => x.Created)
            .IsRequired();

        model.HasIndex(x => x.Id)
            .IsUnique();

        model.Property(x => x.Model)
            .IsRequired();

        model.Property(x => x.Brand)
            .IsRequired();

        model.Property(x => x.Value)
            .IsRequired();
    }
}