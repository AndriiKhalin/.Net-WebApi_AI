using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class HardDriveController : Controller
{
    private IHardDriveRepository _hardRepo;

    public HardDriveController(IHardDriveRepository hardRepo)
    {
        _hardRepo = hardRepo;
    }

    public async Task<IActionResult> Index(SortState sortState)
    {
        var sort = new SortViewModel<HardDrive>(sortState);


        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;

        var hardDrives = await _hardRepo.GetAll();
        hardDrives = sort.SortList(hardDrives, sortState);

        return View(hardDrives);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(HardDrive hardDrive)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _hardRepo.Add(hardDrive);
                TempData["successMessage"] = "HardDrive created successfully";
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
            HardDrive hardDrive = await _hardRepo.GetById(id);

            if (hardDrive != null)
            {
                return View(hardDrive);
            }

            TempData["errorMessage"] = $"HardDrive details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(HardDrive hardDrive)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _hardRepo.Update(hardDrive);
                TempData["successMessage"] = "HardDrive Update Successfully";
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
            HardDrive hardDrive = await _hardRepo.GetById(id);

            if (hardDrive != null)
            {
                return View(hardDrive);
            }

            TempData["errorMessage"] = $"Hard Drive details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(HardDrive hardDrive)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _hardRepo.Delete(hardDrive.Id);
                TempData["successMessage"] = "Hard Drive Deleted Successfully";
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