using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class UnitController : Controller
{

    private IUnitRepository _unitRepo;

    public UnitController(IUnitRepository unitRepo)
    {
        _unitRepo = unitRepo;
    }

    public async Task<IActionResult> Index(SortState sortState)
    {
        var sort = new SortViewModel<Unit>(sortState);


        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;

        var units = await _unitRepo.GetAll();
        units = sort.SortList(units, sortState);

        return View(units);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Unit unit)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _unitRepo.Add(unit);
                TempData["successMessage"] = "Unit Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["errorMessage"] = "Model is invalid";
                return View();
            }
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View();
        }

    }


    [HttpGet]
    public async Task<IActionResult> Update(int id = 0)
    {

        if (id == 0)
        {
            return BadRequest();
        }
        else
        {
            Unit unit = await _unitRepo.GetById(id);

            if (unit != null)
            {
                return View(unit);
            }

            TempData["errorMessage"] = $"Unit details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(Unit unit)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _unitRepo.Update(unit);
                TempData["successMessage"] = "Unit Update Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["errorMessage"] = "Model is Invalid";
                return View();
            }
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View();
        }

    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id = 0)
    {

        if (id == 0)
        {
            return BadRequest();
        }
        else
        {
            Unit unit = await _unitRepo.GetById(id);

            if (unit != null)
            {
                return View(unit);
            }

            TempData["errorMessage"] = $"Unit details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(Unit unit)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _unitRepo.Delete(unit.Id);
                TempData["successMessage"] = "Unit Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["errorMessage"] = "Model is Invalid";
                return View();
            }
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View();
        }

    }
}