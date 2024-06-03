using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Problem;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace judge.system.core.Service.Impls
{
    public class ProblemService : IProblemService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProblemService(Context context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResponse<List<GetProblemRes>>> GetAll()
        {
            var problems = await _context.Problems.ToListAsync();
            List<GetProblemRes> res = new List<GetProblemRes>();
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);


            foreach (var item in problems)
            {
                List<string> tagName = new List<string>();
                var problem = _mapper.Map<GetProblemRes>(item);

                var problemTag = await _context.ProblemTags.Where(x => x.ProblemId == item.ProblemId).ToListAsync();
                foreach (var tag in problemTag)
                {
                    var name = _context.Tags.FirstOrDefaultAsync(y => y.TagId == tag.TagId).Result;
                    tagName.Add(name.TagName);

                }

                problem.TagId = tagName;
                res.Add(problem);
            }

            return new APIResponse<List<GetProblemRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }

        public async Task<APIResponse<GetProblemRes>> GetById(int problemId)
        {
            var problem = await _context.Problems.FindAsync(problemId);

            if (problem == null)
            {
                return new APIResponse<GetProblemRes>
                {
                    StatusCode = 404,
                    Message = "Problem not found",
                };
            }

            var item = _mapper.Map<GetProblemRes>(problem);

            return new APIResponse<GetProblemRes>
            {
                StatusCode = 200,
                Message = "Success",
                Data = item
            };
        }

        public Task<APIResponse<GetProblemDetailRes>> GetProblemDetail(int id)
        {
            throw new NotImplementedException();
        }
    }
}
