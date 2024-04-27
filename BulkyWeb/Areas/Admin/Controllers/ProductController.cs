﻿using Bulky.DataAccess.Repository;
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
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var productList = _productRepository.GetAll();

            return View(productList);
        }

        public IActionResult Create()
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

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productModel)
        {
           
            if (!ModelState.IsValid)
                return View();

            _productRepository.Add(productModel.Product);
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
