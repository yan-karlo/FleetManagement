using FleetManagement.Domain.Entities;

namespace FleetManagement.Domain.Interfaces.Repositories
{
    public interface IColorRepository
    {
        Task<IEnumerable<Color>> GetAllColors();
        Task<IEnumerable<Color>> GetColors(int offSet, int recordsQty);
        Task<Color> GetColorById(int id);
        Task<int> AddAsync(Color color);
        
        Task RemoveAsync(Color color);
        Task UpdateAsync(Color color);
    }
}
