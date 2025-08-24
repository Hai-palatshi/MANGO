using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using static Mango.web.Models.Utility.SD;


namespace Mango.web.Service
{
    public class BaseService : IBaseService
    {
        private readonly ITokenProvier tokenProvier;
        private readonly IHttpClientFactory httpClientFactory;
        public BaseService(IHttpClientFactory _httpClientFactory, ITokenProvier _tokenProvier)
        {
            httpClientFactory = _httpClientFactory;
            tokenProvier = _tokenProvier;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = httpClientFactory.CreateClient("MangoAPI");
                
                //HttpClient client = new HttpClient();
                //HttpClient client = httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token

                if (withBearer )
                {
                    var token = tokenProvier.GetToken();
                    message.Headers.Add("Authorization",$"Bearer {token}");    
                }

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.
                    SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;
                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "NOT Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = " internal Server Error" };
                    default:
                        var apiContet = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
                        return apiResponseDto;

                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = true
                };
                return dto;
            }

        }


    }
}
