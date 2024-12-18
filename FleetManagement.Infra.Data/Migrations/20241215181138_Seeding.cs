using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagement.Infra.Data.Migrations
{
    /// Seeding the tables  />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            // Colors Seeding
            mb.Sql("INSERT INTO Colors(Name) VALUES('Branco')");
            mb.Sql("INSERT INTO Colors(Name) VALUES('Preto')");
            mb.Sql("INSERT INTO Colors(Name) VALUES('Amarelo')");
            mb.Sql("INSERT INTO Colors(Name) VALUES('Vermelho')");
            mb.Sql("INSERT INTO Colors(Name) VALUES('Azul')");
            mb.Sql("INSERT INTO Colors(Name) VALUES('Verde')");

            // VehicleTypes Seeding
            mb.Sql("INSERT INTO VehicleTypes(Name, PassengersCapacity) VALUES('Car',4)");
            mb.Sql("INSERT INTO VehicleTypes(Name, PassengersCapacity) VALUES('Bus',42)");
            mb.Sql("INSERT INTO VehicleTypes(Name, PassengersCapacity) VALUES('Truck',1)");

            // Vehicles Seeding
            mb.Sql("INSERT INTO Vehicles(ChassisSeries, ChassisNumber, ColorId, VehicleTypeId) VALUES('ABCD',11111,1,1)");
            mb.Sql("INSERT INTO Vehicles(ChassisSeries, ChassisNumber, ColorId, VehicleTypeId) VALUES('EFGH',222222,2,2)");
            mb.Sql("INSERT INTO Vehicles(ChassisSeries, ChassisNumber, ColorId, VehicleTypeId) VALUES('IJKL',333333,3,3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Vehicles");
            mb.Sql("DELETE FROM Colors");
            mb.Sql("DELETE FROM VehicleTypes");
        }
    }
}
