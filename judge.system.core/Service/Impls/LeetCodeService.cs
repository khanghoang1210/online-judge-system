using judge.system.core.Database;
using judge.system.core.DTOs.Responses;
using judge.system.core.Models;
using judge.system.core.Service.Interface;
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
            List<Tag> tagList = new List<Tag>();

            foreach (var item in result.LeetCodeProblemRes)
            {
                var existingProblem = _context.Problems.FirstOrDefault(x => x.TitleSlug == item.TitleSlug);
                if (existingProblem == null)
                {
                    var res = new Problem
                    {
                        Title = item.Title,
                        TitleSlug = item.TitleSlug,
                        Difficulty = item.Difficulty,
                        TagId = item.TopicTags.Select(x => x.Id).ToList(),
                    };
                    _context.Problems.Add(res);
                }



                foreach (var tag in item.TopicTags)
                {
                    var existingTag = tagList.FirstOrDefault(x => x.TagId == tag.Id);
                    if (existingTag == null)
                    {
                        var newTag = new Tag
                        {
                            TagId = tag.Id,
                            TagName = tag.Name,
                            TagSlug = tag.Slug,
                        };

                        tagList.Add(newTag);
                        _context.Tags.Add(newTag);

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
