using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Infra.Data.EntitiesConfiguration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            //PK
            builder.HasKey(c => c.Id);

            
            //Fieds
            builder.Property(v => v.ChassisSeries)
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(v => v.ChassisNumber)
                .HasColumnType("bigint")
                .IsRequired();

            //Index
            builder.HasIndex(v => new { v.ChassisSeries, v.ChassisNumber }).IsUnique(); 
            
            //FK
            builder.HasOne(v => v.VehicleType)
                .WithMany(vt => vt.Vehicles)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(v => v.Color)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.ColorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
