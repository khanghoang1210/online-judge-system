using judge.system.core.DTOs.Requests.Account;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Account;

namespace judge.system.core.Service.Interface
{
    public interface IAccountService
    {
        Task<APIResponse<string>> Create(CreateAccountReq req);
        Task<APIResponse<List<ReadAccountsRes>>> ReadAll();
        Task<APIResponse<string>> Update(UpdateAccountReq req, string userName);
    }
}
