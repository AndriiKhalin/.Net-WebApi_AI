using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class MotherBoardController : Controller
{

    private IMotherBoardRepository _motherRepo;

    public MotherBoardController(IMotherBoardRepository motherRepo)
    {
        _motherRepo = motherRepo;
    }

    public async Task<IActionResult> Index(SortState sortState)
    {
        var sort = new SortViewModel<MotherBoard>(sortState);


        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;

        var motherBoards = await _motherRepo.GetAll();
        motherBoards = sort.SortList(motherBoards, sortState);

        return View(motherBoards);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(MotherBoard motherBoard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _motherRepo.Add(motherBoard);
                TempData["successMessage"] = "MotherBoard created successfully";
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
            MotherBoard motherBoard = await _motherRepo.GetById(id);

            if (motherBoard != null)
            {
                return View(motherBoard);
            }

            TempData["errorMessage"] = $"MotherBoard details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(MotherBoard motherBoard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _motherRepo.Update(motherBoard);
                TempData["successMessage"] = "MotherBoard Update Successfully";
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
            MotherBoard motherBoard = await _motherRepo.GetById(id);

            if (motherBoard != null)
            {
                return View(motherBoard);
            }

            TempData["errorMessage"] = $"MotherBoard details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(MotherBoard motherBoard
    )
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _motherRepo.Delete(motherBoard.Id);
                TempData["successMessage"] = "MotherBoard Deleted Successfully";
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