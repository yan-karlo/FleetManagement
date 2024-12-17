using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces.Repositories;

namespace FleetManagement.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<string> AddAsync(VehicleInputDTO vehicleInputDTO)
        {
            var vehicleEntity = _mapper.Map<Vehicle>(vehicleInputDTO);
            var vehicleEntityChassis = await _vehicleRepository.AddAsync(vehicleEntity);
            return vehicleEntityChassis;
        }

        public async Task<VehicleOutputDTO> GetVehicleById(int id)
        {
            var vehicleEntity = await _vehicleRepository.GetVehicleById(id);
            return _mapper.Map<VehicleOutputDTO>(vehicleEntity);
        }

        public async Task<IEnumerable<VehicleOutputDTO>> GetVehicles(int offSet, int recordsQty)
        {
            var vehicleEntityList = await _vehicleRepository.GetVehicles(offSet, recordsQty);
            return _mapper.Map<IEnumerable<VehicleOutputDTO>>(vehicleEntityList);
        }

        public async Task RemoveAsync(int id)
        {
            await _vehicleRepository.RemoveAsync(new Vehicle(id, "", 0, 0, 0));
        }

        public async Task UpdateAsync(VehicleInputDTO vehicle)
        {
            var vehicleEntity = _mapper.Map<Vehicle>(vehicle);
            await _vehicleRepository.UpdateAsync(vehicleEntity);
        }
    }
}
