using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Submission;


namespace judge.system.core.Service.Interface
{
    public class ISubmissionService
    {
        Task<APIResponse<List<GetSubmissionRes>>> GetNNearest(string UserName, int size);
    }
}
