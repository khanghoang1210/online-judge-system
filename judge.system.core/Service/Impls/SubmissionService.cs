using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Requests.Account;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Submission;
using judge.system.core.Models;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace judge.system.core.Service.Impls
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmissionService(Context context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            //var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private int? GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue("Id");

            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            return null;
        }

        public async Task<APIResponse<List<string>>> GetLanguageList()
        {
            var currentUser = GetCurrentUserId();

            if (currentUser == null)
            {
                return new APIResponse<List<string>>
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    Data = null,
                };

            }

            var languages = await _context.Submissions
                .Where(s => s.UserId == currentUser && s.IsAccepted)
                .Select(s => s.Language)
                .Distinct()
                .ToListAsync();

            //var LanguageList = languages.Select(
            //    lang => new GetLanguageSubmittedRes { Language = lang }
            //    ).ToList();

            return new APIResponse<List<string>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = languages
            };
        }

        public async Task<APIResponse<List<GetSubmissionRes>>> GetAllSubmission(string userName)
        {
            int size = 20;

            var user = _context.Accounts.FirstOrDefault(a => a.UserName == userName);

            if (user.Id == null)
            {
                return new APIResponse<List<GetSubmissionRes>>
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    Data = null,
                };

            }

            var submissions = await _context.Submissions
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Problem)
                .Take(size)
                .ToListAsync();

            var res = submissions.Select(submission => new GetSubmissionRes
            {
                ProblemTitle = submission.Problem.Title,
                Time = submission.CreatedAt,
                IsAccepted = submission.IsAccepted,
                NumCasesPassed = submission.NumCasesPassed,
                Language = submission.Language
            }).ToList();

            return new APIResponse<List<GetSubmissionRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }

        public async Task<APIResponse<bool>> CreateSubmission(CreateSubmissionReq req)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var item = new Submission
                {
                    ProblemId = req.ProblemId,
                    Account = _context.Accounts.FirstOrDefault(x => x.UserName == currentUser),
                    IsAccepted = req.IsAccepted,
                    NumCasesPassed = req.NumCasesPassed,
                    Language = req.Language,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Submissions.Add(item);
                await _context.SaveChangesAsync();
                return new APIResponse<bool>
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<bool>
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Data = false,
                };
            }
        }

    }
}
