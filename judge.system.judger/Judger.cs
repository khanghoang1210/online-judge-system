using judge.system.Helpers;
using judge.system.judger.Comparers;
using judge.system.models;
using judge.system.models.Judgers;

namespace judge.system.judger
{
    public static class Judger
    {
        public static IReadOnlyDictionary<ProgrammingLanguage, JudgerLangConfig> DefaultLangConfigs = new Dictionary<ProgrammingLanguage, JudgerLangConfig>
        {
            [ProgrammingLanguage.C] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = new Command
                {
                    Name = "gcc",
                    Arguments = new string[]
                            {
                                Compiler.V_CodeFile,
                                "-o",
                                Compiler.V_Output
                            }
                },
                RunCommand = new Command
                {
                    Name = Judger.V_CompileOutput,
                }
            },
            [ProgrammingLanguage.Cpp] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = new Command
                {
                    Name = "g++",
                    Arguments = new string[]
                            {
                                Compiler.V_CodeFile,
                                "-o",
                                Compiler.V_Output
                            }
                },
                RunCommand = new Command
                {
                    Name = Judger.V_CompileOutput,
                }
            },
            [ProgrammingLanguage.Java] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = new Command
                {
                    Name = "javac",
                    Arguments = new string[]
                            {
                                Compiler.V_CodeFile
                            }
                },
                RunCommand = new Command
                {
                    Name = "java",
                    Arguments = new string[]
                            {
                                "Main"
                            },
                }
            },
            [ProgrammingLanguage.CSharp] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = new Command
                {
                    Name = "csc",
                    Arguments = new string[]
                            {
                                Compiler.V_CodeFile
                            }
                },
                RunCommand = new Command
                {
                    Name = Judger.V_CompileOutput,
                }
            },

            [ProgrammingLanguage.Python] = new JudgerLangConfig
            {
                CompileCommand = null,
                RunCommand = new Command
                {
                    Name = "python",
                    Arguments = new string[]
                            {
                                Judger.V_CodeFile
                            },
                }
            },

            [ProgrammingLanguage.Go] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = new Command
                {
                    Name = "go",
                    Arguments = new string[]
                            {
                                "build",
                                Compiler.V_CodeFile
                            }
                },
                RunCommand = new Command
                {
                    Name = Judger.V_CompileOutput
                }
            },

            [ProgrammingLanguage.Javascript] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = null,
                RunCommand = new Command
                {
                    Name = "node",
                    Arguments = new string[]
                            {
                                Judger.V_CodeFile
                            }
                }
            },
            [ProgrammingLanguage.Java] = new JudgerLangConfig
            {
                CompileMemoryLimit = 1024 * 1024 * 1024,
                CompileTimeLimit = TimeSpan.FromSeconds(20),
                CompileCommand = new Command
                {
                    Name = "kotlinc",
                    Arguments = new string[]
                            {
                                Compiler.V_CodeFile
                            }
                },
                RunCommand = new Command
                {
                    Name = "kotlin",
                    Arguments = new string[]
                            {
                                "Main"
                            },
                }
            },

        };

        public const string V_CodeFile = "{codefile}", V_CompileOutput = "{compiled}";

        public static async Task<JudgeResult> Judge(string name, Command executor, string workingDirectory, TimeSpan timeLimit, long memoryLimit, TextReader input, TextReader output, IJudgeComparer comparer)
        {
            JudgeResult res = new JudgeResult
            {
                Id = name,
                State = JudgeState.Pending,
                Issues = new List<Issue>()
            };
            try
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(executor.Name, string.Join(" ", executor.Arguments))
                {
                    WorkingDirectory = workingDirectory
                };
                using (Runner runner = new Runner(startInfo)
                {
                    TimeLimit = timeLimit,
                    MemoryLimit = memoryLimit,
                    Input = input,
                })
                {
                    res.State = JudgeState.Judging;
                    await runner.Run();
                    res.Time = runner.RunningTime;
                    res.Memory = runner.MaximumMemory;
                    if (!string.IsNullOrEmpty(runner.Error))
                        res.Issues.Add(new Issue(IssueLevel.Warning, "Error output: " + runner.Error));
                    switch (runner.State)
                    {
                        case RunnerState.Ended:
                            {
                                if (runner.ExitCode != 0)
                                {
                                    res.Issues.Add(new Issue(IssueLevel.Error, $"exited with {runner.ExitCode}."));
                                    res.State = JudgeState.RuntimeError;
                                    break;
                                }
                                TextReader expected = output;
                                StringReader real = new StringReader(runner.Output ?? "");
                                Issue[] diff = comparer.Compare(expected, real).ToArray();
                                if (diff.Length != 0)
                                {
                                    res.Issues.AddRange(diff);
                                    res.State = JudgeState.WrongAnswer;
                                }
                                else
                                {
                                    res.State = JudgeState.Accepted;
                                }
                                break;
                            }
                        case RunnerState.OutOfMemory:
                            {
                                string message = $"Used {runner.MaximumMemory} bytes, limit {memoryLimit} bytes.";
                                res.Issues.Add(new Issue(IssueLevel.Error, $"Memory limit exceeded for {name}. {message}"));
                                res.State = JudgeState.MemoryLimitExceeded;
                                break;
                            }
                        case RunnerState.OutOfTime:
                            {
                                string message = $"Used {runner.RunningTime.TotalSeconds} seconds, limit {timeLimit.TotalSeconds} seconds.";
                                res.Issues.Add(new Issue(IssueLevel.Error, $"Time limit exceeded for {name}. {message}"));
                                res.State = JudgeState.TimeLimitExceeded;
                                break;
                            }
                        default:
                            throw new Exception("The program doesn't stop.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.Issues.Add(new Issue(IssueLevel.Error, $"System error for {name} with {ex.ToString()}."));
                res.State = JudgeState.SystemError;
            }
            return res;
        }
    }
}
