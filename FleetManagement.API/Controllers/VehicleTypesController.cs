using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;
        public VehicleTypesController(IVehicleTypeService vehicleTypeService)
        {
            _vehicleTypeService = vehicleTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleTypeDTO>>> GetAll()
        {
            try
            {
                var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypes();
                if (vehicleTypes == null) return NotFound("VehicleTypess not found.");
                return Ok(vehicleTypes);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("byid/{id?}")]
        public async Task<ActionResult<VehicleTypeDTO>> GetById(int? id)
        {
            if (id <= 0) return BadRequest("VehicleTypes id is required.");

            try
            {
                var vehicleType = await _vehicleTypeService.GetVehicleTypeById(id.Value);
                if (vehicleType == null) return NotFound("VehicleTypes not found.");
                return Ok(vehicleType);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(VehicleTypeInputDTO vehicleTypeInputDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var vehicleTypeId = await _vehicleTypeService.AddAsync(vehicleTypeInputDTO);
                return Created("", vehicleTypeId);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VehicleTypeDTO vehicleTypeDTO)
        {
            if (!ModelState.IsValid || vehicleTypeDTO.Id <= 0) return BadRequest(ModelState);

            try
            {
                await _vehicleTypeService.UpdateAsync(vehicleTypeDTO);
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _vehicleTypeService.RemoveAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
