using judge.system.core.DTOs.Requests.Problem;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Problem;
using judge.system.core.Models;

namespace judge.system.core.Service.Interface
{
    public interface IProblemService
    {
        Task<APIResponse<Problem>>  GetById(int id);
        Task<APIResponse<List<Problem>>> ReadAll();
    }
}
