namespace judge.system.models
{
    public enum JudgeState
    {
        Pending,
        Compiling,
        Judging,
        Accepted,
        WrongAnswer,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        RuntimeError,
        CompileError,
        SystemError,
    }
}
