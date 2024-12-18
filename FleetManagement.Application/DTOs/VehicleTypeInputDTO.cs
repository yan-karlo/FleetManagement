using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Application.DTOs
{
    public class VehicleTypeInputDTO
    {
        [Required(ErrorMessage = "Te vehicle type name is required.")]
        [MinLength(3)]
        [MaxLength(30)]
        [DisplayName("Vehicle Type Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The vehicle type passengers capacity is required.")]
        [DisplayName("Passengers Capacity")]
        [Range(1, 100)]
        public int PassengersCapacity { get; set; }
    }
}
