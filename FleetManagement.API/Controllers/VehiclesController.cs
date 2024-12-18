using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Presentation.DTOs;
using FleetManagement.Presentation.Presenters;
using FleetManagement.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private IVehicleService _vehicleService { get; set; }

        public VehiclesController(IVehicleService vehicleservice)
        {
            _vehicleService = vehicleservice;
        }

        [HttpGet("{offSet?}/{recordsQty?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VehicleOutputDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> GetVehicles(int offSet = 0, int recordsQty = 20)
        {
            if (offSet < 0 || recordsQty < 1) return ErrorResultFactory.handle(new ArgumentException("The off-set and records quantity must be greater or equals than 0 (zero)."));

            var result = new ReadPresenter<IEnumerable<VehicleOutputDTO>>(() => _vehicleService.GetVehicles(offSet, recordsQty));
            return await result.Execute();
        }

        [HttpGet("getbyid/{id?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> GetVehicleById(int? id)
        {
            if (id <= 0) return ErrorResultFactory.handle(new ArgumentException("No Id to be researched."));

            var result = new ReadPresenter<VehicleOutputDTO>(() => _vehicleService.GetVehicleById(id.Value));
            return await result.Execute();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Add(VehicleInputDTO vehicleInputDTO)
        {
            if (!ModelState.IsValid) return ErrorResultFactory.handle(new ArgumentException("Invalid Data."));

            var result = new CreatePresenter<int>(() => _vehicleService.AddAsync(vehicleInputDTO));
            return await result.Execute();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Update(VehicleOutputDTO vehicleOutputDTO)
        {
            if (!ModelState.IsValid || vehicleOutputDTO.Id <= 0) return ErrorResultFactory.handle(new ArgumentException("Invalid data.")); ;

            var result = new UpdatePresenter(() => _vehicleService.UpdateAsync(vehicleOutputDTO));
            return await result.Execute();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Remove(int id)
        {
            if (id <= 0) return ErrorResultFactory.handle(new ArgumentException("Vehicle id is required."));

            var result = new RemovePresenter(() => _vehicleService.RemoveAsync(id));
            return await result.Execute();
        }

    }
}
