using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.WebUI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("colors")]
    public class ColorsController : Controller
    {
        private IColorService _colorService { get; set; }

        public ColorsController(IColorService colorservice)
        {
            _colorService = colorservice;
        }

        #region READ
        [HttpGet("{offSet?}/{recordsQty?}")]
        public async Task<IActionResult> Index(int offSet = 0, int recordsQty = 20)
        {
            if (offSet < 0 || recordsQty < 1)
            {
                ModelState.AddModelError(string.Empty, "The off-set and records quantity must be greater or equals than 0 (zero).");
                return View();
            };

            try
            {
                var colors = await _colorService.GetColors(offSet, recordsQty);
                return View(colors);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when getting the color to be detailed: " + e.Message);
                return View();
            }
        }
        #endregion

        #region CREATE
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ColorDTO colorDTO)
        {
            var colorName = colorDTO.Name ?? "";
            if (string.IsNullOrEmpty(colorName))
            {
                ModelState.AddModelError(string.Empty, "The color name is required and it cannot be less than 1.");
                return View(colorDTO);
            }

            try
            {
                await _colorService.AddAsync(colorName);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when creating a color: " + e.Message);
                return View(colorDTO);
            }
        }
        #endregion

        #region UPDATE
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int? id = 0)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "The id is required and it cannot be less than 1.");
                return View();
            }

            try
            {
                var colorDTO = await _colorService.GetColorById(id ?? -1);
                if (colorDTO == null)
                {
                    ModelState.AddModelError(string.Empty, "The color to be edited was not found.");
                    return View();
                }

                return View(colorDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when locating the color to edition: " + e.Message);
                return View();
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ColorDTO colorDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(colorDTO);
            }

            try
            {
                await _colorService.UpdateAsync(colorDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when updating the color: " + e.Message);
                return View(colorDTO);
            }
        }
        #endregion

        #region DELETE
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "The id is required and it cannot be less than 1.");
                return View();
            }

            try
            {
                var colorDTO = await _colorService.GetColorById(id ?? -1);
                if (colorDTO == null)
                {
                    ModelState.AddModelError(string.Empty, "The color to be deleted was not found.");
                    return View();
                }

                return View(colorDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when locating the color to be deleted: " + e.Message);
                return View();
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(ColorDTO colorDTO)
        {

            if (!ModelState.IsValid)
            {
                return View(colorDTO);
            }

            try
            {
                await _colorService.RemoveAsync(colorDTO.Id.Value);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when deleting the color: " + e.Message);
                return View(colorDTO);
            }
        }
        #endregion

        #region DETAILS
        [HttpGet("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "The id is required and it cannot be less than 1.");
                return View();
            }
            try
            {
                var colorDTO = await _colorService.GetColorById(id ?? -1);
                if (colorDTO == null)
                {
                    ModelState.AddModelError(string.Empty, "Color not found.");
                    return View();
                }
                return View(colorDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when locating the color: " + e.Message);
                return RedirectToAction("Index");
            }

        }
        #endregion
    }
}
