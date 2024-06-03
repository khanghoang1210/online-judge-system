using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Problem;

namespace judge.system.core.Service.Interface
{
    public interface IProblemService
    {
        Task<APIResponse<GetProblemRes>> GetById(int id);
        Task<APIResponse<List<GetProblemRes>>> GetAll();
        Task<APIResponse<GetProblemDetailRes>> GetProblemDetail(int id);
    }
}
