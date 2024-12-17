using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<VehicleOutputDTO>>> GetVehicles(int offSet = 0, int recordsQty = 20)
        {
            if (offSet < 0 || recordsQty < 1) return BadRequest("The off-ser and records quantity must be greater or equals than 0 (zero).");

            try
            {
                var vehicles = await _vehicleService.GetVehicles(offSet, recordsQty);
                if (vehicles == null) return NotFound("Vehicles not found.");

                return Ok(vehicles);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getbyid/{id?}")]
        public async Task<ActionResult<VehicleOutputDTO>> GetVehicleById(int? id)
        {
            if (id <= 0) return NotFound("No Id to be researched.");

            try
            {
                var vehicle = await _vehicleService.GetVehicleById(id.Value);
                if (vehicle == null) return NotFound("Vehicles not found.");
                return Ok(vehicle);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<string>> Add(VehicleInputDTO vehicleInputDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Data.");

            try
            {
                var vehicleId = await _vehicleService.AddAsync(vehicleInputDTO);
                if (vehicleId == null) return Conflict("Vehicle not added.");
                return Ok(vehicleId);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VehicleOutputDTO vehicleOutputDTO)
        {
            if (!ModelState.IsValid || vehicleOutputDTO.Id <= 0) return BadRequest("Invalid Data.");
            try
            {
                await _vehicleService.UpdateAsync(vehicleOutputDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Remove(int id)
        {
            if (id <= 0) return BadRequest("Vehicle id is required.");
            try
            {
                await _vehicleService.RemoveAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}
