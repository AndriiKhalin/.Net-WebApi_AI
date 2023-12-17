using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class ComponentComputerController : Controller
{

    private IComponentComputerRepository _compRepo;

    public ComponentComputerController(IComponentComputerRepository compRepo)
    {
        _compRepo = compRepo;
    }

    public async Task<IActionResult> Index()
    {
        var componentComputers = await _compRepo.GetAll();
        return View(componentComputers);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ComponentComputer componentComputer)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _compRepo.Add(componentComputer);
                TempData["successMessage"] = "Component created successfully";
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
            ComponentComputer componentComputer = await _compRepo.GetById(id);

            if (componentComputer != null)
            {
                return View(componentComputer);
            }

            TempData["errorMessage"] = $"Component Computer details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(ComponentComputer componentComputer)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _compRepo.Update(componentComputer);
                TempData["successMessage"] = "Component Computer Update Successfully";
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
            ComponentComputer componentComputer = await _compRepo.GetById(id);

            if (componentComputer != null)
            {
                return View(componentComputer);
            }

            TempData["errorMessage"] = $"Component Computer details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(ComponentComputer componentComputer)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _compRepo.Delete(componentComputer.Id);
                TempData["successMessage"] = "Component Computer Deleted Successfully";
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