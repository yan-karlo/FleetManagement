using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.WebUI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("vehicletypes")]
    public class VehicleTypesController : Controller
    {
        private IVehicleTypeService _vehicleTypeService { get; set; }

        public VehicleTypesController(IVehicleTypeService vehicleTypeservice)
        {
            _vehicleTypeService = vehicleTypeservice;
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
                var vehicleTypes = await _vehicleTypeService.GetVehicleTypes(offSet, recordsQty);
                return View(vehicleTypes);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when getting the vehicle types: " + e.Message);
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
        public async Task<IActionResult> Create(VehicleTypeInputDTO vehicleTypeInputDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicleTypeInputDTO);
            }

            try
            {
                await _vehicleTypeService.AddAsync(vehicleTypeInputDTO);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An error happened when creating the vehicle type: {e.Message}");
                return View(vehicleTypeInputDTO);
            }
        }
        #endregion

        #region UPDATE
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int? id = 0)
        {
            if (id == null || id.Value <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Please, inform a valid Id.");
                return View();
            }

            try
            {
                var vehicleTypeDTO = await _vehicleTypeService.GetVehicleTypeById(id ?? -1);
                if (vehicleTypeDTO == null)
                {
                    ModelState.AddModelError(string.Empty, $"An error happened when locating the vehicle type to be edited.");
                    return View();
                }

                return View(vehicleTypeDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An error happened when locating the vehicle type to be edited: {e.Message}");
                return View();
            }

        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(VehicleTypeDTO vehicleTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicleTypeDTO);
            }
            try
            {
                await _vehicleTypeService.UpdateAsync(vehicleTypeDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An error happened when updating the vehicle type: {e.Message}");
                return View(vehicleTypeDTO);
            }

        }
        #endregion

        #region DELETE
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(VehicleTypeDTO vehicleTypeDTO)
        {
            var id = vehicleTypeDTO?.Id ?? -1;
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Please, inform a valid Id.");
                return View();
            }

            try
            {
                var locatedVehicleTypeDTO = await _vehicleTypeService.GetVehicleTypeById(id);
                if (locatedVehicleTypeDTO == null)
                {
                    ModelState.AddModelError(string.Empty, $"An error happened when locating the vehicle type to be deleted.");
                    return View();
                }

                return View(locatedVehicleTypeDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An error happened when locating the vehicle type to be deleted: {e.Message}");
                return View();
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Please, inform a valid Id.");
                return View();
            }

            try
            {
                await _vehicleTypeService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An error happened when deleting the vehicle type: {e.Message}");
                return View();
            }
        }
        #endregion

        #region DETAILS
        [HttpGet("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Please, inform a valid Id.");
                return View();
            }

            try
            {
                var vehicleTypeDTO = await _vehicleTypeService.GetVehicleTypeById(id ?? -1);
                if (vehicleTypeDTO == null)
                {
                    ModelState.AddModelError(string.Empty, $"An error happened when locating the vehicle type to be detailed.");
                    return View();
                }
;
                return View(vehicleTypeDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"An error happened when locating the vehicle type: {e.Message}");
                return View();
            }
        }
        #endregion

    }
}
