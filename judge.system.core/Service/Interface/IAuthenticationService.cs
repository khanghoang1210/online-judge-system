using judge.system.core.DTOs.Requests.Authentication;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Authentication;

namespace judge.system.core.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<APIResponse<LoginRes>> Login(LoginReq req);
    }
}
