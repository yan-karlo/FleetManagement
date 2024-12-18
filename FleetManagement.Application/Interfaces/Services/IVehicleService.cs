using FleetManagement.Application.DTOs;
using System.Numerics;

namespace FleetManagement.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleOutputDTO>> GetVehicles(int offSet, int recordsQty);
        Task<VehicleOutputDTO> GetVehicleById(int id);
        Task<int> AddAsync(VehicleInputDTO vehicle);

        Task RemoveAsync(int id);
        Task UpdateAsync(VehicleInputDTO vehicle);
    }
}
