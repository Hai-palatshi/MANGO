namespace Mango.web.Service.IService
{
    public interface ITokenProvier
    {
        void SetToken(string token);
        string GetToken();  
        void ClearToken();  
    }
}
