using judge.system.core.Database;
using judge.system.core.DTOs.Requests.Problem;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Problem;
using judge.system.core.Models;
using judge.system.core.Service.Interface;
using System.Reflection;


namespace judge.system.core.Service.Impls
{
    public class ProblemService : IProblemService
    {
        private readonly Context _context;
        public ProblemService(Context context) 
        {
            _context = context;
        }

        public async Task<APIResponse<List<GetProblemRes>>> GetAll()
        {
            var problems = _context.Problems.ToList();
            List<GetProblemRes> res = new List<GetProblemRes>();

            foreach (var problem in problems)
            {
                GetProblemRes item = new GetProblemRes();

                item.ProblemId = problem.ProblemId;
                item.Title = problem.Title;
                item.Description = problem.Description;
                item.TimeLimit = problem.TimeLimit;
                item.MemoryLimit = problem.MemoryLimit;

                res.Add(item);
            }

            return new APIResponse<List<GetProblemRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }

        public async Task<APIResponse<GetProblemRes>> GetById(int inputId)
        {
            var problem = await _context.Problems.FindAsync(inputId);

            if (problem == null)
            {
                return new APIResponse<GetProblemRes>
                {
                    StatusCode = 404,
                    Message = "Problem not found",
                };
            }

            var item = new GetProblemRes()
            {
                ProblemId = problem.ProblemId,
                Title = problem.Title,
                Description = problem.Description,
                TimeLimit = problem.TimeLimit,
                MemoryLimit = problem.MemoryLimit
            };

            return new APIResponse<GetProblemRes>
            {
                StatusCode = 200,
                Message = "Success",
                Data = item
            };
        }

        public Task<APIResponse<Problem>> GetById(string id)
        {
            throw new NotImplementedException();
        }

        Task<APIResponse<List<Problem>>> IProblemService.ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}
