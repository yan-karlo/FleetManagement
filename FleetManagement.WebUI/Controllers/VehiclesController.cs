using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FleetManagement.WebUI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("vehicles")]
    public class VehiclesController : Controller
    {
        private IVehicleService _vehicleService { get; set; }
        private IColorService _colorService { get; set; }
        private IVehicleTypeService _vehicleTypeService { get; set; }

        public VehiclesController(IVehicleService vehicleService, IColorService colorService, IVehicleTypeService vehicleTypeService)
        {
            _vehicleService = vehicleService;
            _colorService = colorService;
            _vehicleTypeService = vehicleTypeService;
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
                var vehicles = await _vehicleService.GetVehicles(offSet, recordsQty);
                return View(vehicles);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when getting the vehicles: " + e.Message);
                return View();
            }
        }
        #endregion

        #region CREATE
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.ColorId = new SelectList(await _colorService.GetAllColors(), "Id", "Name");
                var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypes();
                var selectListItems = vehicleTypes.Select(vt => new SelectListItem
                {
                    Value = vt.Id.ToString(),
                    Text = $"name: {vt.Name} | capacity: {vt.PassengersCapacity} passenger(s)"
                });
                ViewBag.VehicleTypeId = new SelectList(selectListItems, "Value", "Text");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when mountting the view for a vehicle creation: " + e.Message);
                return View();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(VehicleInputDTO vehicleInputDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _vehicleService.AddAsync(vehicleInputDTO);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "An error happened when creating a vehicle: " + e.Message);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The mounted vehicle is not a valid one. Please, try again.");
            }

            ViewBag.ColorId = new SelectList(await _colorService.GetAllColors(), "Id", "Name");
            var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypes();
            var selectListItems = vehicleTypes.Select(vt => new SelectListItem
            {
                Value = vt.Id.ToString(),
                Text = $"name: {vt.Name} | capacity: {vt.PassengersCapacity} passenger(s)"
            });
            ViewBag.VehicleTypeId = new SelectList(selectListItems, "Value", "Text");
            return View(vehicleInputDTO);

        }
        #endregion

        #region UPDATE
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please, inform a valid Id.");
                return View();
            };

            try
            {
                var vehicleOutputDTO = await _vehicleService.GetVehicleById(id.Value);
                if (vehicleOutputDTO == null)
                {
                    ModelState.AddModelError(string.Empty, "No vehicle was found by this id.");
                    return View();
                };

                ViewBag.ColorId = new SelectList(await _colorService.GetAllColors(), "Id", "Name", vehicleOutputDTO.ColorId);

                var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypes();
                var selectListItems = vehicleTypes.Select(vt => new SelectListItem
                {
                    Value = vt.Id.ToString(),
                    Text = $"name: {vt.Name} | capacity: {vt.PassengersCapacity} passenger(s)"
                });
                ViewBag.VehicleTypeId = new SelectList(selectListItems, "Value", "Text", vehicleOutputDTO.VehicleTypeId);

                return View(vehicleOutputDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when locating the vehicle to be edited: " + e.Message);
                return View();
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(VehicleOutputDTO vehicleOutputDTO)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please, inform a valid vehicle to edited. ");
                return View(vehicleOutputDTO);
            }

            try
            {
                await _vehicleService.UpdateAsync((VehicleInputDTO)vehicleOutputDTO);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when updating the vehicle: " + e.Message);
            }

            try
            {
                ViewBag.ColorId = new SelectList(await _colorService.GetAllColors(), "Id", "Name", vehicleOutputDTO.ColorId);
                var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypes();
                var selectListItems = vehicleTypes.Select(vt => new SelectListItem { Value = vt.Id.ToString(), Text = $"name: {vt.Name} | capacity: {vt.PassengersCapacity} passenger(s)" });
                ViewBag.VehicleTypeId = new SelectList(selectListItems, "Value", "Text", vehicleOutputDTO.VehicleTypeId);

                return View(vehicleOutputDTO);

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error affected the vehicle update and all data recovery for mounting this view. More details: " + e.Message);
                return View(vehicleOutputDTO);
            }
        }
        #endregion

        #region DELETE
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please, inform a valid Id.");
                return View();
            };

            try
            {
                var vehicleOutputDTO = await _vehicleService.GetVehicleById(id.Value);
                if (vehicleOutputDTO == null)
                {
                    ModelState.AddModelError(string.Empty, "No vehicle was found by this id.");
                    return View();
                };
                return View(vehicleOutputDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when locating the vehicle to be deleted: " + e.Message);
                return View();
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(VehicleOutputDTO vehicleOutputDTO)
        {
            var id = vehicleOutputDTO.Id;
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please, inform a valid Id.");
                return View(vehicleOutputDTO);
            };

            try
            {
                await _vehicleService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when deleting the vehicle: " + e.Message);
                return View(vehicleOutputDTO);
            }
        }
        #endregion

        #region DETAILS
        [HttpGet("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please, inform a valid Id.");
                return View(new VehicleOutputDTO());
            };

            try
            {
                var vehicleOutputDTO = await _vehicleService.GetVehicleById(id.Value);
                if (vehicleOutputDTO == null)
                {
                    ModelState.AddModelError(string.Empty, "No vehicle was found by this id.");
                    return View(new VehicleOutputDTO());
                }

                return View(vehicleOutputDTO);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error happened when locating the vehicle to be detailed: " + e.Message);
                return View(new VehicleOutputDTO());
            }
        }
        #endregion
    }
}
