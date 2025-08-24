using AutoMapper;
using Azure;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext db;
        private readonly ResponseDto response;
        private IMapper mapper;
        public CouponAPIController(AppDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
            response = new ResponseDto();
        }

        [HttpGet]
        public object Get()
        {
            try
            {
                IEnumerable<Coupon> objList = db.coupons.ToList();
                response.Result = mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public object Get(int id)
        {
            try
            {
                Coupon obj = db.coupons.First(x => x.CoupinId == id);
                response.Result = mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public object Get(string code)
        {
            try
            {
                Coupon obj = db.coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());

                response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = mapper.Map<Coupon>(couponDto);
                db.coupons.Add(obj);
                db.SaveChanges();

                response.Result = mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]

        public ResponseDto put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = mapper.Map<Coupon>(couponDto);
                db.coupons.Update(obj);
                db.SaveChanges();

                response.Result = mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = db.coupons.First(x=> x.CoupinId == id);
                db.coupons.Remove(obj);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
