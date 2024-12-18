using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Infra.Data.EntitiesConfiguration
{
    public class VehicleTypeConfiguration: IEntityTypeConfiguration<VehicleType>
    {

        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Name)
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .IsRequired();
            builder.Property( vt => vt.PassengersCapacity)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
