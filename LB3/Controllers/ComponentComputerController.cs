using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LB3.Controllers;

public class ComponentComputerController : Controller
{

    private IComponentComputerRepository _compRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<ComponentComputer> PaginateViewModel { get; set; }


    public ComponentComputerController(IComponentComputerRepository compRepo, IConfiguration configuration)
    {
        _compRepo = compRepo;
        Configuration = configuration;

    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var componentComputer = await _compRepo.GetById(id);
        if (componentComputer == null)
        {
            return NotFound();
        }
        else
        {
            return View(componentComputer);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var componentComputer = await _compRepo.GetAll();

        var sort = new SortViewModel<ComponentComputer>(sortState);


        if (pageIndex == null || pageIndex < 1)
        {
            pageIndex = 1;
        }


        if (sortState != null && (sortState != SortState.None))
        {
            componentComputer = sort.SortList(componentComputer, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {

            componentComputer = SearchViewModel<ComponentComputer>.Search(componentComputer, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<ComponentComputer>(componentComputer, pageIndex, pageSize);
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


        componentComputer = await PaginateViewModel.CreateAsync();

        return View(componentComputer);
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