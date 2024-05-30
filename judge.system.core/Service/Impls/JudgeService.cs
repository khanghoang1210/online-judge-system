using judge.system.core.Database;
using judge.system.core.DTOs.Requests.Judge;
using judge.system.core.DTOs.Responses.Judge;
using judge.system.core.Service.Interface;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<int>> GetInOut(int id)
        {
            try
            {
                var problem = await _context.ProblemDetails.FirstOrDefaultAsync(p => p.ProblemId == id);
                var input = problem.TestCases.Select(x => x.Input);
                foreach (var testCase in input)
                {

                }
                return input.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SubmitCodeRes> Submit(SubmitCodeReq submission)
        {

            var sourceFilePath = GetSourceFilePath(submission.Language, submission.SourceCode);

            var result = await RunCode(sourceFilePath, submission.Language);

            File.Delete(sourceFilePath);

            return result;
        }


        private string GetSourceFilePath(string language, string sourceCode)
        {
            string fullSourceCode = "";
            var tempPath = Path.GetTempFileName();
            string extension = language switch
            {
                "python" => ".py",
                "cpp" => ".cpp",
                "java" => ".java",
                "javascript" => ".js",
                _ => throw new NotSupportedException("Unsupported language")
            };

            if (language == "java")
            {
                fullSourceCode = @$"public class Solution{{

	                                public static void main(String[] args){{

		                                System.out.println(add(1,5));
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
            else if (language == "cpp")
            {
                fullSourceCode = $@"#include <iostream>
                                using namespace std;
                                {sourceCode}
                                int main() {{
                                  
                                    cout << add(1,2) << endl;

                                    return 0;
                                }}";

                tempPath = Path.ChangeExtension(tempPath, extension);
            }
            else if (language == "python")
            {
                fullSourceCode = $"{sourceCode}" +
                    $"\nprint(add(1,2))";
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
                case "python":
                    processInfo.FileName = "python";
                    processInfo.Arguments = filePath;
                    break;

                case "cpp":
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

                case "java":
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
