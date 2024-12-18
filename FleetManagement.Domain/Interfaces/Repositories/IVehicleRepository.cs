using FleetManagement.Domain.Entities;

namespace FleetManagement.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehicles(int offSet, int recordsQty);
        Task<Vehicle?> GetVehicleById(int id);
        Task<int> AddAsync(Vehicle vehicle);

        Task<int> RemoveAsync(Vehicle vehicle);
        Task<int> UpdateAsync(Vehicle vehicle);
    }
}
