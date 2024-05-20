using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Problem;
using judge.system.core.Service.Interface;


namespace judge.system.core.Service.Impls
{
    public class ProblemService : IProblemService
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        public ProblemService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<GetProblemRes>>> GetAll()
        {
            var problems = _context.Problems.ToList();
            List<GetProblemRes> res = new List<GetProblemRes>();

            foreach (var item in problems)
            {
                //GetProblemRes problem = new GetProblemRes();
                //problem.ProblemId = item.ProblemId;
                //problem.Title = item.Title;
                //problem.TitleSlug = item.TitleSlug;
                //problem.Difficulty = item.Difficulty;
                //problem.TagId = item.TagId;
                var problem = _mapper.Map<GetProblemRes>(item);
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

            //var item = new GetProblemRes()
            //{
            //    ProblemId = problem.ProblemId,
            //    Title = problem.Title,
            //    Description = problem.Description,
            //    TimeLimit = problem.TimeLimit,
            //    MemoryLimit = problem.MemoryLimit
            //};

            return new APIResponse<GetProblemRes>
            {
                StatusCode = 200,
                Message = "Success",
                Data = item
            };
        }
    }
}
