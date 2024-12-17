using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;
        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetAll()
        {
            try
            {
                var colors = await _colorService.GetAllColors();
                if (colors == null) return NotFound("Colors not found.");

                return Ok(colors);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("byid/{id?}")]
        public async Task<ActionResult<ColorDTO>> GetById(int? id)
        {
            if (id <= 0) return BadRequest("Color id is required.");

            try
            {
                var color = await _colorService.GetColorById(id.Value);
                if (color == null) return NotFound("Color not found.");

                return Ok(color);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(string colorName)
        {
            if (string.IsNullOrEmpty(colorName)) return BadRequest(ModelState);

            try
            {
                var colorId = await _colorService.AddAsync(colorName);
                return Ok(colorId);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(ColorDTO colorDTO)
        {
            if (!ModelState.IsValid || colorDTO.Id <= 0) return BadRequest("Invalid data.");

            try
            {
                await _colorService.UpdateAsync(colorDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Remove(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _colorService.RemoveAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
