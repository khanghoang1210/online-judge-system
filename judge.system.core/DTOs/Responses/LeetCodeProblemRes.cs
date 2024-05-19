using Newtonsoft.Json;

namespace judge.system.core.DTOs.Responses
{
    public class TopicTag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public class LeetCodeProblemRes
    {
        [JsonProperty("acRate")]
        public double AcRate { get; set; }

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("freqBar")]
        public object FreqBar { get; set; }

        [JsonProperty("questionFrontendId")]
        public string QuestionFrontendId { get; set; }

        [JsonProperty("isFavor")]
        public bool IsFavor { get; set; }

        [JsonProperty("isPaidOnly")]
        public bool IsPaidOnly { get; set; }

        [JsonProperty("status")]
        public object Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("titleSlug")]
        public string TitleSlug { get; set; }

        [JsonProperty("topicTags")]
        public List<TopicTag> TopicTags { get; set; }

        [JsonProperty("hasSolution")]
        public bool HasSolution { get; set; }

        [JsonProperty("hasVideoSolution")]
        public bool HasVideoSolution { get; set; }
    }
    public class ProblemsetQuestionList
    {
        [JsonProperty("totalQuestions")]
        public int TotalQuestions { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("problemsetQuestionList")]
        public List<LeetCodeProblemRes> LeetCodeProblemRes { get; set; }
    }
}

