using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.Models;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace judge.system.core.Service.Impls
{
    public class LeetCodeService : ILeetCodeService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly Context _context;
        public LeetCodeService(IConfiguration configuration, HttpClient httpClient, Context context)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _context = context;
        }

        public Task<string> GetDailyProblemAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetProblemListAsync(int limit = 50, string[] tags = null)
        {
            var url = $"http://localhost:3000/problems?limit={limit}";
            if (tags != null && tags.Length > 0)
            {
                var tagsParam = string.Join("+", tags);
                url += $"&tags={tagsParam}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProblemsetQuestionList>(data);

            foreach (var item in result.LeetCodeProblemRes)
            {
                var existingProblem = _context.Problems
                                              .Include(p => p.ProblemTags)
                                              .ThenInclude(pt => pt.Tag)
                                              .FirstOrDefault(x => x.TitleSlug == item.TitleSlug);
                if (existingProblem == null)
                {
                    existingProblem = new Problem
                    {
                        Title = item.Title,
                        TitleSlug = item.TitleSlug,
                        Difficulty = item.Difficulty,
                        ProblemTags = new List<ProblemTag>()
                    };
                    _context.Problems.Add(existingProblem);
                }

                foreach (var tagDto in item.TopicTags)
                {
                    var existingTag = _context.Tags.Local.FirstOrDefault(x => x.TagId == tagDto.Id)
                                     ?? _context.Tags.FirstOrDefault(x => x.TagId == tagDto.Id);

                    if (existingTag == null)
                    {
                        existingTag = new Tag
                        {
                            TagId = tagDto.Id,
                            TagName = tagDto.Name,
                            TagSlug = tagDto.Slug,
                        };
                        _context.Tags.Add(existingTag);
                    }

                    if (!existingProblem.ProblemTags.Any(pt => pt.TagId == tagDto.Id))
                    {
                        existingProblem.ProblemTags.Add(new ProblemTag
                        {
                            Problem = existingProblem,
                            Tag = existingTag
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return url;
        }


        public Task<string> GetSingleProblemAsync(string titleSlug)
        {
            throw new NotImplementedException();
        }
    }
}
