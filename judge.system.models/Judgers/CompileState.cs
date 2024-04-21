namespace judge.system.models.Judgers
{
    public enum CompileState
    {
        Pending,
        Compiling,
        Compiled,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        RuntimeError,
        SystemError,
    }
}
