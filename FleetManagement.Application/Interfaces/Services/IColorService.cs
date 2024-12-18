using FleetManagement.Application.DTOs;

namespace FleetManagement.Application.Interfaces.Services
{
    public interface IColorService
    {
        Task<IEnumerable<ColorDTO>> GetAllColors();
        Task<IEnumerable<ColorDTO>> GetColors(int offSet, int recordsQty);
        Task<ColorDTO> GetColorById(int id);
        Task<int> AddAsync(string colorName);

        Task RemoveAsync(int id);
        Task UpdateAsync(ColorDTO color);
    }
}
