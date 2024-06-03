using AutoMapper;
using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Submission;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
            
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private int? GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            return null;
        }

        public async Task<APIResponse<List<string>>> GetLanguageList()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == null)
            {
                return new APIResponse<List<string>>
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    Data = null,
                };

            }

            var languages = await _context.Submissions
                .Where(s => s.UserId == currentUserId && s.IsAccepted)
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

        public async Task<APIResponse<List<GetSubmissionRes>>> GetNNearest(int size, Models.Submission submission)
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == null)
            {
                return new APIResponse<List<GetSubmissionRes>>
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    Data = null,
                };

            }

            var submissions = await _context.Submissions
                .Where(x => x.UserId == currentUserId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(size)
                .ToListAsync();

            var res = submissions.Select(submission =>
            {
                var submissionRes = _mapper.Map<GetSubmissionRes>(submission);
                submissionRes.Time = DateTime.Now - submission.CreatedAt;
                return submissionRes;
            }).ToList();

            return new APIResponse<List<GetSubmissionRes>>
            {
                StatusCode = 200,
                Message = "Success",
                Data = res
            };
        }


    }
}
