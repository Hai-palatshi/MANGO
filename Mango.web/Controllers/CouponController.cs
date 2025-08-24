using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;
        public CouponController(ICouponService _couponService)
        {
            couponService = _couponService;
        }
        [HttpGet]
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto?> list = new();
            ResponseDto? response = await couponService.GetAllCouponAsync();
            if (response != null && response.IsSuccess && response.Result != null)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await couponService.CreateCouponAsync(model);
                if (response != null && response.IsSuccess && response.Result != null)
                {
                    TempData["success"] = "Coupon Created";

                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int id)
        {
            ResponseDto? response = await couponService.GetByIdCouponAsync(id);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? response = await couponService.DeleteCouponAsync(couponDto.CoupinId);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                TempData["error"] = response?.Message;
                return View(couponDto);
            }
            else
            {
                TempData["success"] = "Coupon Deleted";
                return RedirectToAction(nameof(CouponIndex));
            }
        }
    }
}
