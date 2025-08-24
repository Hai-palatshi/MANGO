using Mango.web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Mango.Services.web.Models;
using Mango.web.Service.IService;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using IdentityModel;


namespace Mango.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        public HomeController(ICartService _cartService, ILogger<HomeController> logger, IProductService _productService)
        {
            _logger = logger;
            productService = _productService;
            cartService = _cartService;
        }

        public async Task<IActionResult> Index()
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

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? model = new();
            ResponseDto? response = await productService.GetByIdProductAsync(productId);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject).FirstOrDefault()?.Value,
                }
            };

            CardDetailDto cardDetails = new CardDetailDto()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };
            List<CardDetailDto> cardDetailsList = new() { cardDetails };
            cartDto.CardDetail = cardDetailsList;

            ResponseDto? response = await cartService.UpsertAsync(cartDto);

            if (response != null && response.IsSuccess && response.Result != null)
            {
                TempData["success"] = "item added to the shopping cart successfully!";
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
