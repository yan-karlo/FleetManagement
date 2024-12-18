using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using FleetManagement.Presentation.Presenters;
using FleetManagement.Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;
using FleetManagement.Presentation.Utilities;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ColorDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> GetAll()
        {
            var result = new ReadPresenter<IEnumerable<ColorDTO>>(() => _colorService.GetAllColors());
            return await result.Execute();
        }

        [HttpGet("byid/{id?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ColorDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id <= 0) return ErrorResultFactory.handle(new ArgumentException("Color id is required."));

            var result = new ReadPresenter<ColorDTO>(() => _colorService.GetColorById(id.Value));
            return await result.Execute();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Add(string colorName)
        {
            if (string.IsNullOrEmpty(colorName)) return BadRequest(ModelState);

            var result = new CreatePresenter<int>(() => _colorService.AddAsync(colorName));
            return await result.Execute();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Update(ColorDTO colorDTO)
        {
            if (!ModelState.IsValid || colorDTO.Id <= 0) return ErrorResultFactory.handle(new ArgumentException("Invalid data."));

            var result = new UpdatePresenter(() => _colorService.UpdateAsync(colorDTO));
            return await result.Execute();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> Remove(int id)
        {
            if (id <= 0) return ErrorResultFactory.handle(new ArgumentException("Invalid id."));

            var result = new RemovePresenter(() => _colorService.RemoveAsync(id));
            return await result.Execute();
        }
    }
}
