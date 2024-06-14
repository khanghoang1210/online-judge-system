using judge.system.core.DTOs.Requests.Account;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Submission;


namespace judge.system.core.Service.Interface
{
    public interface ISubmissionService
    {
        Task<APIResponse<List<GetSubmissionRes>>> GetAllSubmission(string userName);
        Task<APIResponse<bool>> CreateSubmission(CreateSubmissionReq req);
    }
}
