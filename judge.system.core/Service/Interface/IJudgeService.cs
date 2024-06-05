using judge.system.core.DTOs.Requests.Judge;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Judge;

namespace judge.system.core.Service.Interface
{
    public interface IJudgeService
    {
        Task<APIResponse<SubmitCodeRes>> Submit(SubmitCodeReq submitCodeReq);
        Task<List<TestCaseRes>> GetInOut(int id);
    }
}
