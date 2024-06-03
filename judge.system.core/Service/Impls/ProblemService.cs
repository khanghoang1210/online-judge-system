using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Problem;
using judge.system.core.Helper.Converter;
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
        private readonly IJudgeService _judgeService;
        public ProblemService(Context context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IJudgeService judgeService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _judgeService = judgeService;
        }

        public async Task<APIResponse<List<GetProblemRes>>> GetAll()
        {
            var problems = await _context.Problems
                .Include(p => p.ProblemTags)
                    .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = problems.Select(problem => new GetProblemRes
            {
                ProblemId = problem.ProblemId,
                Title = problem.Title,
                TitleSlug = problem.TitleSlug,
                Difficulty = problem.Difficulty,
                TagId = problem.ProblemTags.Select(pt => pt.Tag.TagName).ToList()
            }).ToList();

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

        public async Task<APIResponse<GetProblemDetailRes>> GetProblemDetail(int problemId)
        {
            try
            {
                var problemDetail = await _context.ProblemDetails.FirstOrDefaultAsync(x => x.ProblemId == problemId);
                var item = new GetProblemDetailRes
                {
                    Title = problemDetail.Title,
                    Description = problemDetail.Description,
                    TestCases = await _judgeService.GetInOut(problemId),
                    FunctionName = Converter.ToPascalCase(problemDetail.Title),
                };

                return new APIResponse<GetProblemDetailRes>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = item
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<GetProblemDetailRes>
                {
                    StatusCode = 500,
                    Message = ex.Message,

                };
            }
        }
    }
}
