using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class MotherBoardController : Controller
{

    private IMotherBoardRepository _motherRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<MotherBoard> PaginateViewModel { get; set; }

    public MotherBoardController(IMotherBoardRepository motherRepo, IConfiguration configuration)
    {
        _motherRepo = motherRepo;
        Configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var motherBoard = await _motherRepo.GetById(id);
        if (motherBoard == null)
        {
            return NotFound();
        }
        else
        {
            return View(motherBoard);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var motherBoard = await _motherRepo.GetAll();

        var sort = new SortViewModel<MotherBoard>(sortState);

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
            motherBoard = sort.SortList(motherBoard, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            //query = query.Where(p => p.Name.Contains(searchString));
            motherBoard = SearchViewModel<MotherBoard>.Search(motherBoard, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<MotherBoard>(motherBoard, pageIndex, pageSize);
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


        motherBoard = await PaginateViewModel.CreateAsync();

        return View(motherBoard);
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