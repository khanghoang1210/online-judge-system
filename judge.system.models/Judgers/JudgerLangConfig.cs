using judge.system.Helpers;
using System.ComponentModel.DataAnnotations;

namespace judge.system.models.Judgers
{
    public class JudgerLangConfig
    {
        [DataType(DataType.Duration)]
        public TimeSpan CompileTimeLimit { get; set; }

        public long CompileMemoryLimit { get; set; }

        public Command CompileCommand { get; set; }

        public Command RunCommand { get; set; }
    }
}
