using FleetManagement.Domain.Entities;

namespace FleetManagement.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehicles(int offSet, int recordsQty);
        Task<Vehicle?> GetVehicleById(int id);
        Task<string> AddAsync(Vehicle vehicle);

        Task RemoveAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
    }
}
