using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Submission;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace judge.system.core.Service.Impls
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;

        public SubmissionService(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<GetSubmissionRes>>> GetNNearest(string UserName, int size)
        {
            var submissions = await _context.Submissions.ToListAsync();
            List<GetSubmissionRes> res = new List<GetSubmissionRes>();


            foreach (var item in submissions)
            {
                var submission = _mapper.Map<GetSubmissionRes>(item);
                res.Add(submission);
                if (size == 1)
                {
                    break;
                }
                size--;
            }

            return new APIResponse<List<GetSubmissionRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }
    }
}
