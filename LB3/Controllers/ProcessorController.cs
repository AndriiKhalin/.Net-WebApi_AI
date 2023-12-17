using LB3.Features;
using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class ProcessorController : Controller
{

    private IProcessorRepository _procRepo;

    public ProcessorController(IProcessorRepository procRepo)
    {
        _procRepo = procRepo;
    }

    public async Task<IActionResult> Index(SortState sortState)
    {


        var sort = new SortViewModel<Processor>(sortState);



        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;



        var processors = await _procRepo.GetAll();

        processors = sort.SortList(processors, sortState);

        return View(processors);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Processor processor)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _procRepo.Add(processor);
                TempData["successMessage"] = "Processor created successfully";
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
            Processor processor = await _procRepo.GetById(id);

            if (processor != null)
            {
                return View(processor);
            }

            TempData["errorMessage"] = $"Processor details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(Processor processor)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _procRepo.Update(processor);
                TempData["successMessage"] = "Processor Update Successfully";
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
            Processor processor = await _procRepo.GetById(id);

            if (processor != null)
            {
                return View(processor);
            }

            TempData["errorMessage"] = $"Processor details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(Processor processor)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _procRepo.Delete(processor.Id);
                TempData["successMessage"] = "Processor Deleted Successfully";
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