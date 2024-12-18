using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace FleetManagement.Domain.Entities
{
    public class Vehicle : Entity
    {
        public long ChassisNumber { get; set; }
        public string ChassisSeries { get; set; }
        public string Chassis
        {
            get
            {
                return $"{ChassisSeries}{ChassisNumber}";
            }
        }

        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color Color { get; set; }

        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        public Vehicle() { }

        public Vehicle(int id, string chassisSeries, int chassisNumber, int colorId, int vehicleTypeId)
        {
            Id = id;
            ChassisSeries = chassisSeries;
            ChassisNumber = chassisNumber;
            ColorId = colorId;
            VehicleTypeId = vehicleTypeId;
        }

    }
}
