
using Microsoft.AspNetCore.Mvc;

namespace LB3.Controllers
{
    public class ComputerController : Controller
    {
        private IComputerRepository _compRepo;
        private readonly IConfiguration Configuration;

        public int pageIndex = 1;
        public int totalPages = 0;
        public PaginateViewModel<Computer> PaginateViewModel { get; set; }
        public ComputerController(IComputerRepository compRepo, IConfiguration configuration)
        {
            _compRepo = compRepo;
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CalculatePerformance(int id)
        {

            var computer = await _compRepo.GetById(id);



            if (computer == null)
            {
                return NotFound();
            }


            float performance = _compRepo.CalculatePerformance(computer.ComponentComputer.Processor);

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
            var computer = await _compRepo.GetById(id);
            if (computer == null)
            {
                return NotFound();
            }
            else
            {
                return View(computer);
            }

        }



        public async Task<IActionResult> Index(int pageIndex, string searchString, SortState sortState)
        {
            var computer = await _compRepo.GetAll();

            var sort = new SortViewModel<Computer>(sortState);



            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }


            if (sortState != null && (sortState != SortState.None))
            {
                computer = sort.SortList(computer, sort.Current);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                //query = query.Where(p => p.Name.Contains(searchString));
                computer = SearchViewModel<Computer>.Search(computer, searchString);
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            PaginateViewModel = new PaginateViewModel<Computer>(computer, pageIndex, pageSize);
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


            computer = await PaginateViewModel.CreateAsync();

            return View(computer);
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
