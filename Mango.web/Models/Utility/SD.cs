namespace Mango.web.Models.Utility
{
    public class SD
    {
        public static string CouponAPIBase {  get; set; }
        public static string ProductAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        
        public const string RouleAdmin = "ADMIN";
        public const string RouleCustomer = "CUSTOMER";
        public const string TookenCookie = "JWTToken";


        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
