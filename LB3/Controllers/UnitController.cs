using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class UnitController : Controller
{

    private IUnitRepository _unitRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<Unit> PaginateViewModel { get; set; }


    public UnitController(IUnitRepository unitRepo, IConfiguration configuration)
    {
        _unitRepo = unitRepo;
        Configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var unit = await _unitRepo.GetById(id);
        if (unit == null)
        {
            return NotFound();
        }
        else
        {
            return View(unit);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var unit = await _unitRepo.GetAll();

        var sort = new SortViewModel<Unit>(sortState);

        //IQueryable<Processor> query = _context.Processors;


        //IdSort = sort.IdSort;
        //NameSort = sort.NameSort;
        //PriceSort = sort.PriceSort;
        //CurrentSort = sort.Current;
        //SearchString =
        //CurrentPage = pageIndex;


        if (pageIndex == null || pageIndex < 1)
        {
            pageIndex = 1;
        }


        if (sortState != null && (sortState != SortState.None))
        {
            unit = sort.SortList(unit, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            //query = query.Where(p => p.Name.Contains(searchString));
            unit = SearchViewModel<Unit>.Search(unit, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<Unit>(unit, pageIndex, pageSize);
        this.pageIndex = (int)PaginateViewModel.PageIndex;
        totalPages = PaginateViewModel.TotalPages;

        //decimal count = query.Count();
        //totalPages = (int)Math.Ceiling(count / pageSize);
        //query = query.Skip((this.pageIndex - 1) * pageSize).Take(pageSize);
        //Processor = await query.ToListAsync();
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


        unit = await PaginateViewModel.CreateAsync();

        return View(unit);
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