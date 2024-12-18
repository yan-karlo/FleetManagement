using System.ComponentModel;

namespace FleetManagement.Application.DTOs
{
    public class VehicleOutputDTO : VehicleInputDTO
    {
       [DisplayName("Chassis")]
        public string? Chassis { get; set; }

        [DisplayName("Color Name")]
        public string? ColorName { get; set; }

        [DisplayName("Vehicle Type Name")]
        public string? VehicleTypeName { get; set; }

        [DisplayName("Passengers Capacity")]
        public string? VehicleTypePassengersCapacity { get; set; }

    }
}
