
using judge.system.core.DTOs.Requests.Judge;
using judge.system.core.DTOs.Responses.Judge;
using judge.system.core.Service.Interface;
using System.Diagnostics;

namespace judge.system.core.Service.Impls
{
    public class JudgeService : IJudgeService
    {
        public async Task<SubmitCodeRes> Submit(SubmitCodeReq submission)
        {

            var sourceFilePath = GetSourceFilePath(submission.Language, submission.SourceCode);

            var result = await RunCode(sourceFilePath, submission.Language);

            File.Delete(sourceFilePath);

            return result;
        }


        private string GetSourceFilePath(string language, string sourceCode)
        {
            var tempPath = Path.GetTempFileName();
            string extension = language switch
            {
                "csharp" => ".cs",
                "python" => ".py",
                "cpp" => ".cpp",
                "java" => ".java",
                "javascript" => ".js",
                _ => throw new NotSupportedException("Unsupported language")
            };

            if (language == "java")
            {
                // Tìm tên lớp public trong mã nguồn Java
                var publicClassLine = sourceCode.Split('\n').FirstOrDefault(line => line.Contains("public class"));
                if (publicClassLine != null)
                {
                    var className = publicClassLine.Split(' ')[2];
                    tempPath = Path.Combine(Path.GetTempPath(), $"{className}{extension}");
                }
            }
            else
            {
                tempPath = Path.ChangeExtension(tempPath, extension);
            }

            File.WriteAllText(tempPath, sourceCode);
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
                case "csharp":
                    processInfo.FileName = "dotnet";
                    processInfo.Arguments = $"run {filePath}";
                    break;
                case "javascript":

                    processInfo.FileName = "node";
                    processInfo.Arguments = filePath;
                    break;
                case "python":
                    processInfo.FileName = "python";
                    processInfo.Arguments = filePath;
                    break;

                case "cpp":
                    // Biên dịch C++
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
                    // Biên dịch Java
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
