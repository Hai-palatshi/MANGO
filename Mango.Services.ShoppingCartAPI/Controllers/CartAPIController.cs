using AutoMapper;
using Mango.Services.ShoppingCardAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Mango.Services.ShoppingCartAPI.Models;
using System.Reflection.PortableExecutable;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.SignalR.Protocol;
using Mango.MessageBus;
using Microsoft.Extensions.Configuration;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly AppDbContext dbContext;
        private readonly ResponseDto responseDto;
        private readonly IProductService productService;
        private readonly ICouponctService coupontService;
        private readonly IMessageBus messageBus;
        private readonly IConfiguration configuration;

        public CartAPIController(IConfiguration _configuration, IMessageBus _messageBus, IProductService _productService, IMapper _mapper, AppDbContext _dbContext,
           ICouponctService _coupontService)
        {
            mapper = _mapper;
            dbContext = _dbContext;
            responseDto = new ResponseDto();
            productService = _productService;
            coupontService = _coupontService;
            messageBus = _messageBus;
            configuration = _configuration;
        }



        #region Service Function

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cardtdto)
        {
            try
            {
                var cartFromDb = await dbContext.CartHeader.FirstOrDefaultAsync(x => x.UserId == cardtdto.CartHeader.UserId);
                if (cartFromDb != null)
                {
                    cartFromDb.CoupobCode = cardtdto.CartHeader.CoupobCode;
                    dbContext.CartHeader.Update(cartFromDb);
                    await dbContext.SaveChangesAsync();
                    responseDto.Result = new ResponseDto
                    {
                        Message = "Cart Update with CouponCode",
                        IsSuccess = true,
                        Result = cartFromDb
                    };
                }
            }
            catch (Exception ex)
            {

                responseDto.Message = ex.Message.ToString();
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpDelete("removeCoupon")]
        public async Task<ResponseDto> DeleteCoupon([FromBody] CartDto cardtdto)
        {
            try
            {
                var cartFromDb = await dbContext.CartHeader.FirstOrDefaultAsync(x => x.UserId == cardtdto.CartHeader.UserId);

                if (cartFromDb == null || cartFromDb.CoupobCode.IsNullOrEmpty())
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "CouponCode Not Exist in this User",
                        Result = cardtdto.CartHeader?.UserId
                    };
                }

                cartFromDb.CoupobCode = string.Empty;
                dbContext.CartHeader.Update(cartFromDb);
                await dbContext.SaveChangesAsync();
                responseDto.Result = new ResponseDto
                {
                    Message = $"Coupon remove in this User {cartFromDb.UserId}",
                    IsSuccess = true,
                    Result = cartFromDb
                };
            }
            catch (Exception ex)
            {

                responseDto.Message = ex.Message.ToString();
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> Upsert(CartDto cardDto)
        {
            try
            {
                var cartHeaderFromDb = await dbContext.CartHeader.AsNoTracking().FirstOrDefaultAsync(
                    u => u.UserId == cardDto.CartHeader.UserId);

                if (cartHeaderFromDb == null)
                {
                    // Create a new CartHeader  
                    CartHeader cartHeader = mapper.Map<CartHeader>(cardDto.CartHeader);
                    await dbContext.CartHeader.AddAsync(cartHeader);
                    await dbContext.SaveChangesAsync();
                    cardDto.CardDetail.First().CardHeaderId = cartHeader.CartHeaderId;

                    dbContext.CardDetail.Add(mapper.Map<CardDetail>(cardDto.CardDetail.First()));
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if detail has same product
                    var cartDetailFromDb = await dbContext.CardDetail.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cardDto.CardDetail.First().ProductId &&
                        u.CardHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailFromDb == null)
                    {
                        //create  cartdetail
                        cardDto.CardDetail.First().CardHeaderId = cartHeaderFromDb.CartHeaderId;

                        dbContext.CardDetail.Add(mapper.Map<CardDetail>(cardDto.CardDetail.First()));
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in card detail
                        cardDto.CardDetail.First().Count += cartDetailFromDb.Count;
                        cardDto.CardDetail.First().CardHeaderId = cartDetailFromDb.CardHeaderId;
                        cardDto.CardDetail.First().CardDetailId = cartDetailFromDb.CardDetailId;
                        dbContext.CardDetail.Update(mapper.Map<CardDetail>(cardDto.CardDetail.First()));
                        await dbContext.SaveChangesAsync();
                    }
                }
                responseDto.Result = cardDto;
            }
            catch (Exception)
            {

                throw;
            }
            return responseDto;
        }

        [HttpDelete("DeleteCart")]
        public async Task<ResponseDto> DeleteCart([FromBody] int id)
        {
            var deleteCartDeatailsId = await dbContext.CardDetail
                .FirstOrDefaultAsync(u => u.CardHeaderId == id);

            if (deleteCartDeatailsId == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Cart not found",
                    Result = id
                };
            }

            var deleteCartheder = await dbContext.CartHeader.Where(u => u.CartHeaderId == deleteCartDeatailsId.CardHeaderId).ToListAsync();
            string userId = deleteCartheder.First().UserId ?? "";

            var removeObject = deleteCartheder.ToList().First();
            dbContext.CartHeader.Remove(deleteCartheder.ToList().First());
            dbContext.CardDetail.Remove(deleteCartDeatailsId);
            dbContext.SaveChanges();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Delete succecfully",
                Result = $"user id:{userId}"
            };
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCard(string userId)
        {
            try
            {
                var cardheder = await dbContext.CartHeader.Where(u => u.UserId == userId).ToListAsync();
                var cartdetail = await dbContext.CardDetail.Where(u => u.CardHeaderId == cardheder.FirstOrDefault().CartHeaderId).ToListAsync();

                CartDto card = new()
                {
                    CartHeader = mapper.Map<CardHeaderDto>(cardheder.FirstOrDefault())
                };
                card.CardDetail = mapper.Map<IEnumerable<CardDetailDto>>(cartdetail);
                IEnumerable<ProductDto> productList = await productService.GetProducts();
                foreach (var item in card.CardDetail)
                {
                    item.Product = productList.FirstOrDefault(x => x.ProductId == item.ProductId);
                    card.CartHeader.CardTotal += (item.Count * item.Product.Price);
                }
                //Apply coupon
                if (!card.CartHeader.CoupobCode.IsNullOrEmpty())
                {
                    CouponDto coupon = await coupontService.GetCoupon(card.CartHeader.CoupobCode ?? "");
                    if (coupon != null && card.CartHeader.CardTotal > coupon.MinAmount)
                    {
                        card.CartHeader.CardTotal -= coupon.DiscountAmount;
                        card.CartHeader.Discount = coupon.DiscountAmount;
                    }

                }
                responseDto.Result = card;
            }
            catch (Exception)
            {

                throw;
            }
            return responseDto;
        }


        [HttpPost("EmailCartRequest")]
        public async Task<ResponseDto> EmailCartRequest([FromBody] CartDto cardtdto)
        {
            try
            {
                await messageBus.PublishMessage(cardtdto, configuration.GetValue<string>("TopicAndQueueName:EmailShoppingCartQueue"));

                responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {

                responseDto.Message = ex.Message.ToString();
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        #endregion
    }
}
