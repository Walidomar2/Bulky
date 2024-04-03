namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;   
        }
        public IActionResult Index()
        {
            var categoryList = _context.Categories.ToList();

            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category categoryModel)
        {
            _context.Categories.Add(categoryModel); 
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
