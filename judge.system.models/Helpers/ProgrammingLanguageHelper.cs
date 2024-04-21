namespace judge.system.models.Judgers.Helpers
{
    public static class ProgrammingLanguageHelper
    {
        public static readonly IReadOnlyDictionary<ProgrammingLanguage, string> Extends = new Dictionary<ProgrammingLanguage, string>
        {
            [ProgrammingLanguage.C] = "c",
            [ProgrammingLanguage.Cpp] = "cpp",
            [ProgrammingLanguage.CSharp] = "cs",
            [ProgrammingLanguage.Java] = "java",
            [ProgrammingLanguage.Python] = "py",
            [ProgrammingLanguage.Go] = "go",
            [ProgrammingLanguage.Javascript] = "js",

        };

        public static readonly IReadOnlyDictionary<ProgrammingLanguage, string> DisplayNames = new Dictionary<ProgrammingLanguage, string>
        {
            [ProgrammingLanguage.C] = "C",
            [ProgrammingLanguage.Cpp] = "C++",
            [ProgrammingLanguage.CSharp] = "C#",
            [ProgrammingLanguage.Java] = "Java",
            [ProgrammingLanguage.Python] = "Python",
            [ProgrammingLanguage.Go] = "Go",

            [ProgrammingLanguage.Javascript] = "Javascript",

        };
    }

    public static class JudgeStateHelper
    {
        public static readonly IReadOnlyDictionary<JudgeState, string> DisplayNames = new Dictionary<JudgeState, string>
        {
            [JudgeState.Accepted] = "Accepted",
            [JudgeState.CompileError] = "Compile Error",
            [JudgeState.Compiling] = "Compiling",
            [JudgeState.Judging] = "Judging",
            [JudgeState.MemoryLimitExceeded] = "Memory Limit Exceeded",
            [JudgeState.Pending] = "Pending",
            [JudgeState.RuntimeError] = "Runtime Error",
            [JudgeState.SystemError] = "System Error",
            [JudgeState.TimeLimitExceeded] = "Time Limit Exceeded",
            [JudgeState.WrongAnswer] = "Wrong Answer",
        };
    }
}
