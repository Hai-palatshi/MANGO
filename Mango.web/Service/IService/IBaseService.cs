using Mango.Services.web.Models;
using Mango.web.Models;
using System.Diagnostics.Eventing.Reader;

namespace Mango.web.Service.IService
{
    public interface IBaseService
    {
       Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
