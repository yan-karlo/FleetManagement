using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Presentation.DTOs;
using FleetManagement.Presentation.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/vehicle-types")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;
        public VehicleTypesController(IVehicleTypeService vehicleTypeService)
        {
            _vehicleTypeService = vehicleTypeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VehicleTypeDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> GetAll()
        {
            var result = new ReadPresenter<IEnumerable<VehicleTypeDTO>>(() => _vehicleTypeService.GetAllVehicleTypes());
            return await result.Execute();
        }

        [HttpGet("byid/{id?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleTypeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id <= 0) return BadRequest("VehicleTypes id is required.");

            var result = new ReadPresenter<VehicleTypeDTO>(() => _vehicleTypeService.GetVehicleTypeById(id.Value));
            return await result.Execute();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Add(VehicleTypeInputDTO vehicleTypeInputDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = new CreatePresenter<int>(() => _vehicleTypeService.AddAsync(vehicleTypeInputDTO));
            return await result.Execute();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Update(VehicleTypeDTO vehicleTypeDTO)
        {
            if (!ModelState.IsValid || vehicleTypeDTO.Id <= 0) return BadRequest(ModelState);

            var result = new UpdatePresenter(() => _vehicleTypeService.UpdateAsync(vehicleTypeDTO));
            return await result.Execute();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = new RemovePresenter(() => _vehicleTypeService.RemoveAsync(id));
            return await result.Execute();
        }

    }
}
