using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class HardDriveController : Controller
{
    private IHardDriveRepository _hardRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<HardDrive> PaginateViewModel { get; set; }

    public HardDriveController(IHardDriveRepository hardRepo, IConfiguration configuration)
    {
        _hardRepo = hardRepo;
        Configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var hardDrive = await _hardRepo.GetById(id);
        if (hardDrive == null)
        {
            return NotFound();
        }
        else
        {
            return View(hardDrive);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var hardDrive = await _hardRepo.GetAll();

        var sort = new SortViewModel<HardDrive>(sortState);

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
            hardDrive = sort.SortList(hardDrive, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            //query = query.Where(p => p.Name.Contains(searchString));
            hardDrive = SearchViewModel<HardDrive>.Search(hardDrive, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<HardDrive>(hardDrive, pageIndex, pageSize);
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


        hardDrive = await PaginateViewModel.CreateAsync();

        return View(hardDrive);
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