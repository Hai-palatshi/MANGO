using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }


        [HttpGet]
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto?> list = new();
            ResponseDto? response = await productService.GetAllProductAsync();
            if (response != null && response.IsSuccess && response.Result != null)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await productService.CreateProductAsync(model);
                if (response != null && response.IsSuccess && response.Result != null)
                {
                    TempData["success"] = "Product Created";
                       
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            ResponseDto? response = await productService.GetByIdProductAsync(id);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await productService.DeleteProductAsync(productDto.ProductId);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                TempData["error"] = response?.Message;
				return RedirectToAction(nameof(ProductIndex));
			}
			else
            {
                TempData["success"] = "Coupon Deleted";
                return RedirectToAction(nameof(ProductIndex));
            }
        }



		public async Task<IActionResult> ProductEdit(int ProductId)
		{
			ResponseDto? response = await productService.GetByIdProductAsync(ProductId);
			if (response != null && response.IsSuccess && response.Result != null)
			{
				ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(model);
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            ResponseDto? response = await productService.UpdateProductAsync(productDto);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                TempData["success"] = "product update successully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
            {
                TempData["success"] = "Coupon Deleted";
                return RedirectToAction(nameof(ProductIndex));
            }
        }
    }
}
