
using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers
{
    public class ComputerController : Controller
    {
        private IComputerRepository _compRepo;

        public ComputerController(IComputerRepository compRepo)
        {
            _compRepo = compRepo;
        }

        public async Task<IActionResult> Index(SortState sortState)
        {
            var sort = new SortViewModel<Computer>(sortState);


            ViewData["IdSort"] = sort.IdSort;
            ViewData["NameSort"] = sort.NameSort;
            ViewData["PriceSort"] = sort.PriceSort;
            ViewData["IconId"] = sort.Up_DownId;
            ViewData["IconName"] = sort.Up_DownName;
            ViewData["IconPrice"] = sort.Up_DownPrice;

            var computers = await _compRepo.GetAll();
            computers = sort.SortList(computers, sortState);

            return View(computers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Computer computer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _compRepo.Add(computer);
                    TempData["successMessage"] = "Computer Created Successfully";
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
        public async Task<IActionResult> Update(int id = 0)
        {

            if (id == 0)
            {
                return BadRequest();
            }
            else
            {
                Computer computer = await _compRepo.GetById(id);

                if (computer != null)
                {
                    return View(computer);
                }

                TempData["errorMessage"] = $"Computer details not found with Id : {id}";
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(Computer computer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _compRepo.Update(computer);
                    TempData["successMessage"] = "Computer Update Successfully";
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
                Computer computer = await _compRepo.GetById(id);

                if (computer != null)
                {
                    return View(computer);
                }

                TempData["errorMessage"] = $"Computer details not found with Id : {id}";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Computer computer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _compRepo.Delete(computer.Id);
                    TempData["successMessage"] = "Computor Deleted Successfully";
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
}
