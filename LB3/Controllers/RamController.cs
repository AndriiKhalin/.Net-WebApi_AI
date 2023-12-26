using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class RamController : Controller
{

    private IRamRepository _ramRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<Ram> PaginateViewModel { get; set; }

    public RamController(IRamRepository ramRepo, IConfiguration configuration)
    {
        _ramRepo = ramRepo;
        Configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var ram = await _ramRepo.GetById(id);
        if (ram == null)
        {
            return NotFound();
        }
        else
        {
            return View(ram);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var ram = await _ramRepo.GetAll();

        var sort = new SortViewModel<Ram>(sortState);

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
            ram = sort.SortList(ram, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            //query = query.Where(p => p.Name.Contains(searchString));
            ram = SearchViewModel<Ram>.Search(ram, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<Ram>(ram, pageIndex, pageSize);
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


        ram = await PaginateViewModel.CreateAsync();

        return View(ram);
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