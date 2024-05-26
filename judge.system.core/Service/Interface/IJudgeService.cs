using judge.system.core.DTOs.Requests.Judge;
using judge.system.core.DTOs.Responses.Judge;

namespace judge.system.core.Service.Interface
{
    public interface IJudgeService
    {
        Task<SubmitCodeRes> Submit(SubmitCodeReq submitCodeReq);
    }
}
