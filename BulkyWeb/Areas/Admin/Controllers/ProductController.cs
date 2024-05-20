using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var productList = _productRepository.GetAll(includeProperties: "Category");

            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _categoryRepository.GetAll()
                .ToList()
               .Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }),
                Product = new Product()
            };

            if(id == null || id == 0)
            {
                return View(productVM); //create
            }
            else
            {
                //update
                productVM.Product = _productRepository.Get(c => c.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productModel,IFormFile? img)
        {
           
            if (!ModelState.IsValid)
                return View();

            if(img != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string imgName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                string imgPath = Path.Combine(wwwRootPath, @"images\product");

                // in case you update the image
                if(!string.IsNullOrEmpty(productModel.Product.ImageUrl))
                {
                    var oldImgPath = 
                        Path.Combine(wwwRootPath,productModel.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(imgPath,imgName),FileMode.Create))
                {
                    img.CopyTo(fileStream);
                }

                productModel.Product.ImageUrl = @"\images\product\" + imgName;
            }

            if(productModel.Product.Id == 0)
            {
                _productRepository.Add(productModel.Product);
                TempData["success"] = "Product Created Successfully";
               
            }
            else
            {
                _productRepository.Update(productModel.Product);
                TempData["success"] = "Product Updated Successfully";
            }
            _productRepository.Save();
            return RedirectToAction(nameof(Index));
        }
     
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _productRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

       [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _productRepository.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if(System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);    
            }

            _productRepository.Remove(productToBeDeleted);
            _productRepository.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }
}
