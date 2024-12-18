using FleetManagement.Domain.Entities;

namespace FleetMAnagement.Infra.Data.Test.Helpers
{
    internal class VehicleMockedDataHelper
    {
        public static List<Color> GetColors()
        {
            return new List<Color>
            {
                new Color(1, "Red"),
                new Color(2, "Blue"),
                new Color(3, "Green")
            };
        }

        public static List<Color> GetColorsWithoutIds()
        {
            return new List<Color>
            {
                new Color("Red"),
                new Color("Blue"),
                new Color("Green")
            };
        }

        public static List<VehicleType> GetVehicleTypes()
        {
            return new List<VehicleType>
            {
                new VehicleType(1, "Car", 1),
                new VehicleType(2, "Truck", 2),
                new VehicleType(3, "Motorcycle", 3)
            };
        }

        public static List<VehicleType> GetVehicleTypesWithoutIds()
        {
            return new List<VehicleType>
            {
                new VehicleType("Car", 1),
                new VehicleType("Truck", 2),
                new VehicleType("Motorcycle", 3)
            };
        }

        public static List<Vehicle> GetVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle { Id = 1, ChassisSeries = "ABCD", ChassisNumber = 111, ColorId = 1, VehicleTypeId = 1 },
                new Vehicle { Id = 2, ChassisSeries = "EFGH", ChassisNumber = 222, ColorId = 2, VehicleTypeId = 2 },
                new Vehicle { Id = 3, ChassisSeries = "IJKL", ChassisNumber = 333, ColorId = 3, VehicleTypeId = 3 }
            };
        }

        public static List<Vehicle> GetVehiclesWithoutIds()
        {
            return new List<Vehicle>
            {
                new Vehicle { ChassisSeries = "ABCD", ChassisNumber = 111, ColorId = 1, VehicleTypeId =  1 },
                new Vehicle { ChassisSeries = "EFGH", ChassisNumber = 222, ColorId = 2, VehicleTypeId =  2 },
                new Vehicle { ChassisSeries = "IJKL", ChassisNumber = 333, ColorId = 3, VehicleTypeId =  3 }
            };
        }
    }
}
