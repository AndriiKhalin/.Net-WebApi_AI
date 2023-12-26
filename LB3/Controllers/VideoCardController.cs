using LB3.Features;
using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class VideoCardController : Controller
{

    private IVideoCardRepository _videoRepo;
    private readonly IConfiguration Configuration;

    public int pageIndex = 1;
    public int totalPages = 0;
    public PaginateViewModel<VideoCard> PaginateViewModel { get; set; }

    public VideoCardController(IVideoCardRepository videoRepo, IConfiguration configuration)
    {
        _videoRepo = videoRepo;
        Configuration = configuration;
    }
    [HttpGet]
    public async Task<IActionResult> Detail(int id = 0)
    {

        if (id == 0)
        {
            return NotFound();
        }
        var videoCard = await _videoRepo.GetById(id);
        if (videoCard == null)
        {
            return NotFound();
        }
        else
        {
            return View(videoCard);
        }

    }


    public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
    {
        var videoCard = await _videoRepo.GetAll();

        var sort = new SortViewModel<VideoCard>(sortState);

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
            videoCard = sort.SortList(videoCard, sort.Current);
        }
        if (!string.IsNullOrEmpty(searchString))
        {
            //query = query.Where(p => p.Name.Contains(searchString));
            videoCard = SearchViewModel<VideoCard>.Search(videoCard, searchString);
        }

        var pageSize = Configuration.GetValue("PageSize", 4);
        PaginateViewModel = new PaginateViewModel<VideoCard>(videoCard, pageIndex, pageSize);
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


        videoCard = await PaginateViewModel.CreateAsync();

        return View(videoCard);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(VideoCard videoCard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _videoRepo.Add(videoCard);
                TempData["successMessage"] = "VideoCard Created Successfully";
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
            VideoCard videoCard = await _videoRepo.GetById(id);

            if (videoCard != null)
            {
                return View(videoCard);
            }

            TempData["errorMessage"] = $"VideoCard details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }

    [HttpPost]
    public async Task<IActionResult> Update(VideoCard videoCard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _videoRepo.Update(videoCard);
                TempData["successMessage"] = "VideoCard Update Successfully";
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
            VideoCard videoCard = await _videoRepo.GetById(id);

            if (videoCard != null)
            {
                return View(videoCard);
            }

            TempData["errorMessage"] = $"VideoCard details not found with Id : {id}";
            return RedirectToAction(nameof(Index));
        }

    }


    [HttpPost]
    public async Task<IActionResult> Delete(VideoCard videoCard)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _videoRepo.Delete(videoCard.Id);
                TempData["successMessage"] = "VideoCard Deleted Successfully";
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