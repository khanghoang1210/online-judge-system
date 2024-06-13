using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Post;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;


namespace judge.system.core.Service.Impls
{
    public class PostService : IPostService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public PostService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<GetPostRes>> GetById(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return new APIResponse<GetPostRes>
                {
                    StatusCode = 404,
                    Message = "Post not found",
                };
            }

            var item = _mapper.Map<GetPostRes>(post);

            return new APIResponse<GetPostRes>
            {
                StatusCode = 200,
                Message = "Success",
                Data = item
            };
        }

        public async Task<APIResponse<List<GetPostRes>>> GetAll()
        {
            var posts = await _context.Posts.ToListAsync();
            var res = posts.Select(_mapper.Map<GetPostRes>).ToList();

            return new APIResponse<List<GetPostRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }
    }
}
