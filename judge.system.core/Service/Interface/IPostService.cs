using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Post;

namespace judge.system.core.Service.Interface
{
    public interface IPostService
    {
        Task<APIResponse<List<GetPostRes>>> GetAll();
        Task<APIResponse<GetPostRes>> GetById(int Id);
    }
}
