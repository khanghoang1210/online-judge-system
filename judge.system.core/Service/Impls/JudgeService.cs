using judge.system.core.Database;
using judge.system.core.DTOs.Requests.Judge;
using judge.system.core.DTOs.Responses;
using judge.system.core.DTOs.Responses.Judge;
using judge.system.core.Helper.Converter;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace judge.system.core.Service.Impls
{
    public class JudgeService : IJudgeService
    {
        private readonly Context _context;
        public JudgeService(Context context)
        {
            _context = context;
        }
        public async Task<List<TestCaseRes>> GetInOut(int id)
        {
            try
            {
                var problem = await _context.ProblemDetails.FirstOrDefaultAsync(p => p.ProblemId == id);
                var result = problem.TestCases
                         .Select(testCase => (testCase.Input, testCase.Output))
                         .ToList();

                var json = JsonConvert.SerializeObject(result);
                var test = JsonConvert.DeserializeObject<List<TestCaseRes>>(json);
                return test;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<APIResponse<SubmitCodeRes>> Submit(SubmitCodeReq submission)
        {
            try
            {
                var testCases = await GetInOut(submission.ProblemId);
                var result = new SubmitCodeRes();
                bool isAccept = true;
                var outputs = new List<dynamic>();
                foreach (var testCase in testCases)
                {
                    string param = Converter.DictionaryValuesToString(testCase.Item1);
                    var sourceFilePath = await GetSourceFilePath(submission.Language, submission.SourceCode, submission.ProblemId, param);

                    var response = await RunCode(sourceFilePath, submission.Language);
                    if (response.Success is false)
                    {
                        isAccept = false;
                        return new APIResponse<SubmitCodeRes>
                        {
                            StatusCode = 400,
                            Message = "Compile Error",
                            Data = new SubmitCodeRes
                            {
                                Success = false,
                                Output = response.Errors
                            }
                        };
                    }
                    if (response.Output != testCase.Item2.ToString())
                    {
                        isAccept = false;
                        outputs.Add(response.Output);
                    }
                    outputs.Add(response.Output);
                    File.Delete(sourceFilePath);
                }
                if (!isAccept)
                {
                    return new APIResponse<SubmitCodeRes>
                    {
                        StatusCode = 204,
                        Message = "One or more test case is failed",
                        Data = new SubmitCodeRes
                        {
                            Success = false,
                            Output = outputs[0]
                        }
                    };
                }
                return new APIResponse<SubmitCodeRes>
                {
                    StatusCode = 200,
                    Message = "All test case passed",
                    Data = new SubmitCodeRes
                    {
                        Success = true,
                        Output = outputs[0]
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<SubmitCodeRes>
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }


        }


        private async Task<string> GetSourceFilePath(string language, string sourceCode, int problemId, string param)
        {
            string fullSourceCode = "";
            var tempPath = Path.GetTempFileName();
            var res = await _context.Problems.FirstOrDefaultAsync(p => p.ProblemId == problemId);
            string functionName = Converter.ToPascalCase(res.Title);

            string extension = language switch
            {
                "Python" => ".py",
                "C++" => ".cpp",
                "Java" => ".java",
                "javascript" => ".js",
                _ => throw new NotSupportedException("Unsupported language")
            };

            if (language == "Java")
            {
                fullSourceCode = @$"public class Solution{{

	                                public static void main(String[] args){{

		                                System.out.println({functionName}({param}));
	                                }}
                                    {sourceCode}
                                }}";
                var publicClassLine = fullSourceCode.Split('\n').FirstOrDefault(line => line.Contains("public class"));
                if (publicClassLine != null)
                {
                    var className = publicClassLine.Split(' ')[2];
                    tempPath = Path.Combine(Path.GetTempPath(), $"Solution{extension}");
                }
            }
            else if (language == "C++")
            {
                fullSourceCode = $@"#include <iostream>
                                using namespace std;
                                {sourceCode}
                                int main() {{
                                  
                                    cout << {functionName}({param}) << endl;

                                    return 0;
                                }}";

                tempPath = Path.ChangeExtension(tempPath, extension);
            }
            else if (language == "Python")
            {
                fullSourceCode = $"{sourceCode}" +
                    $"\nprint({functionName}({param}))";
            }
            else
            {
                tempPath = Path.ChangeExtension(tempPath, extension);
            }



            File.WriteAllText(tempPath, fullSourceCode);
            return tempPath;
        }

        private async Task<SubmitCodeRes> RunCode(string filePath, string language)
        {
            var processInfo = new ProcessStartInfo();
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.UseShellExecute = false;


            switch (language)
            {
                case "javascript":

                    processInfo.FileName = "node";
                    processInfo.Arguments = filePath;
                    break;
                case "Python":
                    processInfo.FileName = "python";
                    processInfo.Arguments = filePath;
                    break;

                case "C++":
                    var exeFilePath = Path.ChangeExtension(filePath, ".exe");
                    var compileProcessInfo = new ProcessStartInfo
                    {
                        FileName = "g++",
                        Arguments = $"-o {exeFilePath} {filePath}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false
                    };
                    using (var compileProcess = Process.Start(compileProcessInfo))
                    {
                        await compileProcess.WaitForExitAsync();
                        if (compileProcess.ExitCode != 0)
                        {
                            return new SubmitCodeRes
                            {
                                Success = false,
                                Output = "",
                                Errors = compileProcess.StandardError.ReadToEnd().Replace("\r\n", "")
                            };
                        }
                    }
                    processInfo.FileName = exeFilePath;
                    break;

                case "Java":
                    var compileJavaProcessInfo = new ProcessStartInfo
                    {
                        FileName = "javac",
                        Arguments = filePath,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false
                    };
                    using (var compileJavaProcess = Process.Start(compileJavaProcessInfo))
                    {
                        await compileJavaProcess.WaitForExitAsync();
                        if (compileJavaProcess.ExitCode != 0)
                        {
                            return new SubmitCodeRes
                            {
                                Success = false,
                                Output = "",
                                Errors = compileJavaProcess.StandardError.ReadToEnd().Replace("\r\n", "")
                            };
                        }
                    }
                    processInfo.FileName = "java";
                    processInfo.Arguments = Path.GetFileNameWithoutExtension(filePath);
                    processInfo.WorkingDirectory = Path.GetDirectoryName(filePath);
                    break;

                default:
                    return new SubmitCodeRes
                    {
                        Success = false,
                        Output = "",
                        Errors = "Unsupported language"
                    };
            }


            using var process = Process.Start(processInfo);
            var output = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
            var errors = process.StandardError.ReadToEnd().Replace("\r\n", "");
            process.WaitForExit();

            return new SubmitCodeRes
            {
                Success = process.ExitCode == 0,
                Output = output,
                Errors = errors
            };
        }
    }
}
