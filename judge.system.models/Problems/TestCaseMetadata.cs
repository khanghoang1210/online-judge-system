using judge.system.models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace judge.system.models.Problems
{
    public class TestCaseMetadata : IHasId<string>
    {
        public const long MaximumMemoryLimit = MemoryValueHelper.TB, MinimumMemoryLimit = MemoryValueHelper.MB;

        public string Id { get; set; }

        [DataType(DataType.Duration)]
        public TimeSpan TimeLimit { get; set; }

        [Range(MinimumMemoryLimit, MaximumMemoryLimit)]
        public long MemoryLimit { get; set; }
    }
}
