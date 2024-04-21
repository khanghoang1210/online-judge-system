using judge.system.core.DTOs.Requests.Authentication;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Authentication;
using judge.system.core.Service.Interface;

namespace judge.system.core.Service.Impls
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<APIResponse<LoginRes>> Login(LoginReq req)
        {
            throw new NotImplementedException();
        }
    }
}
