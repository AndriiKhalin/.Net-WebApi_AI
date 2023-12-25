using LB3.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using System.Diagnostics;

namespace LB3.Controllers;

public class ProcessorController : Controller
{

    private IProcessorRepository _procRepo;
    //private readonly MLContext _mlContext;
    //DataViewSchema dataPrepPipelineSchema, modelSchema;
    //private readonly PredictionEngine<Processor, Processor> _predictionEngine;
    public ProcessorController(IProcessorRepository procRepo)
    {
        _procRepo = procRepo;
    }

    [HttpPost]
    public async Task<IActionResult> CalculatePerformance(int id)
    {
        // Получение информации о процессоре по его ID
        var processor = await _procRepo.GetById(id);
        // Здесь используйте вашу ML.NET модель для расчета производительности процессора
        //float performance = _procRepo.CalculatePerformance(processor); // Замените это на ваш вызов ML.NET модели

        //// Здесь можно что-то сделать с результатом performance
        //return RedirectToAction(nameof(Detail), new { id }); // Вернуться на страницу деталей процессора или на другую страницу
        if (processor == null)
        {
            return NotFound();
        }


        // Вызов ML.NET модели для расчета производительности
        float performance = _procRepo.CalculatePerformance(processor); // Замените на свой метод расчета

        TempData["Performance"] = performance.ToString();

        // Перенаправление на страницу Detail для отображения результата
        return RedirectToAction(nameof(Detail), new { id });
        //// Возвращаем результат в формате JSON
        //return Json(new { performance });
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