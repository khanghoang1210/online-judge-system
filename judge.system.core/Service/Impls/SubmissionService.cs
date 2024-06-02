using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Submission;
using judge.system.core.Service.Interface;

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
            var submissions = await _context.Submissions
                .Where(x => x.UserName == UserName)
                .OrderByDescending(x => x.TIME) // Time Submited
                .Take(size)
                .ToListAsync();


            var res = _mapper.Map<List<GetSubmissionRes>>(submissions);

            return new APIResponse<List<GetSubmissionRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }
    }
}
