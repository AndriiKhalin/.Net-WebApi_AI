using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class RamController : Controller
{

    private IRamRepository _ramRepo;

    public RamController(IRamRepository ramRepo)
    {
        _ramRepo = ramRepo;
    }

    public async Task<IActionResult> Index(SortState sortState)
    {

        var sort = new SortViewModel<Ram>(sortState);


        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;

        var rams = await _ramRepo.GetAll();
        rams = sort.SortList(rams, sortState);
        return View(rams);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Ram ram)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _ramRepo.Add(ram);
                TempData["successMessage"] = "Ram created successfully";
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
            Ram ram = await _ramRepo.GetById(id);

            if (ram != null)
            {
                return View(ram);
            }

            TempData["errorMessage"] = $"Ram details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(Ram ram)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _ramRepo.Update(ram);
                TempData["successMessage"] = "Ram Update Successfully";
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
            Ram ram = await _ramRepo.GetById(id);

            if (ram != null)
            {
                return View(ram);
            }

            TempData["errorMessage"] = $"Ram details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(Ram ram)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _ramRepo.Delete(ram.Id);
                TempData["successMessage"] = "Ram Deleted Successfully";
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