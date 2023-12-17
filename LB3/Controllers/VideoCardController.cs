using LB3.Features;
using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers;

public class VideoCardController : Controller
{

    private IVideoCardRepository _videoRepo;

    public VideoCardController(IVideoCardRepository videoRepo)
    {
        _videoRepo = videoRepo;
    }

    public async Task<IActionResult> Index(SortState sortState)
    {
        var sort = new SortViewModel<VideoCard>(sortState);


        ViewData["IdSort"] = sort.IdSort;
        ViewData["NameSort"] = sort.NameSort;
        ViewData["PriceSort"] = sort.PriceSort;
        ViewData["IconId"] = sort.Up_DownId;
        ViewData["IconName"] = sort.Up_DownName;
        ViewData["IconPrice"] = sort.Up_DownPrice;

        var videoCards = await _videoRepo.GetAll();

        videoCards = sort.SortList(videoCards, sortState);
        return View(videoCards);
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