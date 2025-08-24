using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CardDetail, CardDetailDto>();
            CreateMap<CardDetailDto, CardDetail>();

            CreateMap<CartHeader, CardHeaderDto>();
            CreateMap<CardHeaderDto, CartHeader>();
        }
    }
}
