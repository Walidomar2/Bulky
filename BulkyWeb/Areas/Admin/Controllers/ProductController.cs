using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            var productList = _productRepository.GetAll();
            return View(productList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product productModel)
        {
           
            if (!ModelState.IsValid)
                return View();

            _productRepository.Add(productModel);
            _productRepository.Save();

            TempData["success"] = "Product Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var product = _productRepository.Get(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product productModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _productRepository.Update(productModel);
            _productRepository.Save();
            TempData["success"] = "Product Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var product = _productRepository.Get(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var product = _productRepository.Get(x => x.Id == id);

            if (product == null)
                return NotFound();

            _productRepository.Remove(product);
            _productRepository.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
