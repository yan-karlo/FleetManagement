using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FleetManagement.Application.DTOs
{
    public class ColorDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "The color name is required.")]
        [MinLength(4)]
        [MaxLength(30)]
        [DisplayName("Color Name")]
        public string Name { get; set; }
    }
}
