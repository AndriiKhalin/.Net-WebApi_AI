using LB3.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using System.Diagnostics;

namespace LB3.Controllers;

public class ProcessorController : Controller
{

    private IProcessorRepository _procRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<Processor> PaginateViewModel { get; set; }

    public ProcessorController(IProcessorRepository procRepo, IConfiguration configuration)
    {
        _procRepo = procRepo;
        Configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> CalculatePerformance(int id)
    {

        var processor = await _procRepo.GetById(id);

        if (processor == null)
        {
            return NotFound();
        }



        float performance = _procRepo.CalculatePerformance(processor);

        TempData["Performance"] = performance.ToString();


        return RedirectToAction(nameof(Detail), new { id });
    }



    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var processor = await _procRepo.GetById(id);
        if (processor == null)
        {
            return NotFound();
        }
        else
        {
            return View(processor);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var processors = await _procRepo.GetAll();

        var sort = new SortViewModel<Processor>(sortState);




        if (pageIndex == null || pageIndex < 1)
        {
            pageIndex = 1;
        }


        if (sortState != null && (sortState != SortState.None))
        {
            processors = sort.SortList(processors, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            processors = SearchViewModel<Processor>.Search(processors, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<Processor>(processors, pageIndex, pageSize);
        this.pageIndex = (int)PaginateViewModel.PageIndex;
        totalPages = PaginateViewModel.TotalPages;


        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["CurrentSort"] = sort.Current;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;
        ViewData["SearchString"] = searchString;
        ViewData["CurrentPage"] = this.pageIndex;
        ViewData["PaginateViewModel"] = PaginateViewModel;
        ViewData["totalPages"] = totalPages;
        ViewData["pageIndex"] = this.pageIndex;


        processors = await PaginateViewModel.CreateAsync();

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