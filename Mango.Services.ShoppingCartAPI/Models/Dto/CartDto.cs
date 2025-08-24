namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public CardHeaderDto? CartHeader { get; set; }
        public IEnumerable<CardDetailDto>? CardDetail{ get; set; }
    }
}
