using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;


namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var categoryList = _categoryRepository.GetAll();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category categoryModel)
        {
            if (categoryModel.Name == categoryModel.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The Display Order can't exactly match the Name");
            }

            if (!ModelState.IsValid)
                return View();

            _categoryRepository.Add(categoryModel);
            _categoryRepository.Save();

            TempData["success"] = "Category Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var category = _categoryRepository.Get(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _categoryRepository.Update(categoryModel);
            _categoryRepository.Save();
            TempData["success"] = "Category Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var category = _categoryRepository.Get(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var category = _categoryRepository.Get(x => x.Id == id);

            if (category == null)
                return NotFound();

            _categoryRepository.Remove(category);
            _categoryRepository.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
