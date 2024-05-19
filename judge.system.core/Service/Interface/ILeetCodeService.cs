﻿namespace judge.system.core.Service.Interface
{
    public interface ILeetCodeService
    {
        Task<string> GetDailyProblemAsync();
        Task<string> GetSingleProblemAsync(string titleSlug);
        Task<string> GetProblemListAsync(int limit = 100, string[] tags = null);

    }
}
