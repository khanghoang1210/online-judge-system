﻿using judge.system.Helpers;
using judge.system.models.Judgers;


namespace judge.system.judger
{
    public static class Compiler
    {
        public const string V_CodeFile = "{codefile}", V_Output = "{output}";

        public static async Task<CompileResult> Compile(Command executor, string workingDirectory, string codePath, string outputPath, TimeSpan timeLimit, long memoryLimit)
        {
            CompileResult res = new CompileResult
            {
                State = CompileState.Pending,
                Issues = new List<Issue>(),
            };
            try
            {
                Dictionary<string, string> vars = new Dictionary<string, string>()
                {
                    [V_CodeFile] = codePath,
                    [V_Output] = outputPath,
                };
                executor = executor.Resolve(vars);

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(executor.Name, string.Join(" ", executor.Arguments))
                {
                    WorkingDirectory = workingDirectory
                };
                using (Runner runner = new Runner(startInfo)
                {
                    TimeLimit = timeLimit,
                    MemoryLimit = memoryLimit,
                    Input = null,
                })
                {
                    res.State = CompileState.Compiling;
                    await runner.Run();
                    res.Time = runner.RunningTime;
                    res.Memory = runner.MaximumMemory;
                    if (!string.IsNullOrEmpty(runner.Output))
                        res.Issues.Add(new Issue(IssueLevel.Warning, "Compiler output: " + runner.Output));
                    if (!string.IsNullOrEmpty(runner.Error))
                        res.Issues.Add(new Issue(IssueLevel.Warning, "Compiler error output: " + runner.Error));
                    switch (runner.State)
                    {
                        case RunnerState.Ended:
                            {
                                if (runner.ExitCode != 0)
                                {
                                    res.Issues.Add(new Issue(IssueLevel.Error, $"Runtime error: exited with {runner.ExitCode}."));
                                    res.State = CompileState.RuntimeError;
                                    break;
                                }
                                res.State = CompileState.Compiled;
                                res.OutputPath = outputPath;
                                break;
                            }
                        case RunnerState.OutOfMemory:
                            {
                                string message = $"Used {runner.MaximumMemory} bytes, limit {memoryLimit} bytes.";
                                res.State = CompileState.MemoryLimitExceeded;
                                break;
                            }
                        case RunnerState.OutOfTime:
                            {
                                string message = $"Used {runner.RunningTime.TotalSeconds} seconds, limit {timeLimit.TotalSeconds} seconds.";
                                res.State = CompileState.TimeLimitExceeded;
                                break;
                            }
                        default:
                            throw new Exception("The program doesn't stop.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.Issues.Add(new Issue(IssueLevel.Error, $"System error with {ex.ToString()}."));
                res.State = CompileState.SystemError;
            }
            return res;
        }
    }
}
