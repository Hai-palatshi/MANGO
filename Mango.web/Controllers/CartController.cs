using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService; 
        public CartController(ICartService _cartService)
        {
            cartService = _cartService;
        }


        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;   
            var response = await cartService.GetCartByUserIdAsync(userId);

            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto; 
            }
            else
            {
                return new CartDto();
            }
        }

        public async Task<IActionResult> Remove(int CartDetailId)
        {

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await cartService.DeleteFromCartAsync(CartDetailId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart deleted successfully";  
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDetailId)
        {

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var response = await cartService.ApplyCouponAsync(cartDetailId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart deleted successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDetail)
        {
            cartDetail.CartHeader.CoupobCode = "";
            var response = await cartService.ApplyCouponAsync(cartDetail);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart deleted successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDetailId)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value;

            var response = await cartService.EmailCart(cart);
 
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will proccess and send shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return RedirectToAction(nameof(CartIndex));

            }
        }

    }
}
