using AutoMapper;
using Azure;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext db;
        private readonly ResponseDto response;
        private readonly IMapper mapper;
        public ProductAPIController(AppDbContext _db, IMapper _mapper)
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
                IEnumerable<Product> objList = db.Products.ToList();
                response.Result = mapper.Map<IEnumerable<ProductDto>>(objList);
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
                Product obj = db.Products.First(x => x.ProductId == id);
                response.Result = mapper.Map<ProductDto>(obj);

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
        public ResponseDto Post([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = mapper.Map<Product>(ProductDto);
                db.Products.Add(obj);
                db.SaveChanges();

                response.Result = mapper.Map<ProductDto>(obj);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]

        public ResponseDto put([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = mapper.Map<Product>(ProductDto);
                db.Products.Update(obj);
                db.SaveChanges();

                response.Result = mapper.Map<ProductDto>(obj);

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
                Product obj = db.Products.First(x=> x.ProductId == id);
                db.Products.Remove(obj);
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
