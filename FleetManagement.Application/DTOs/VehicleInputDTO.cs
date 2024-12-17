using FleetManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Application.DTOs
{
    public class VehicleInputDTO
    {


        [Required(ErrorMessage = "The chassis series is required.")]
        [MinLength(4)]
        [MaxLength(30)]
        public string ChassisSeries { get; set; }

        [Required(ErrorMessage = "The chassis number is Required")]
        [Range(1, int.MaxValue, ErrorMessage = $"Tha chassis number is expected to be between 1 and 2147483647.")]
        public int ChassisNumber { get; set; }

        [Required(ErrorMessage = "The color id is Required")]
        [Range(1, 999999999)]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "The vehicle type id is Required")]
        [Range(1, 999999999)]
        public int VehicleTypeId { get; set; }

    }
}
