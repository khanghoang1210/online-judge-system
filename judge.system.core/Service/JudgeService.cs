using System.Diagnostics;

namespace judge.system.core.Service
{
    public class JudgeService
    {
        //public static string JudgeC(string code)
        //{

        //    string outputPath = "output.txt";
        //    string errorPath = "error.txt";

        //    string fullCode = $@"
        //                        #include <stdio.h>

        //                        int main() {{
        //                        {code}
        //                            return 0;
        //                        }}
        //                        ";


        //    ProcessStartInfo compileInfo = new ProcessStartInfo
        //    {
        //        FileName = "gcc",
        //        Arguments = "-x c -o tempCode.exe -",
        //        RedirectStandardInput = true,
        //        RedirectStandardError = true
        //    };

        //    using (Process compileProcess = Process.Start(compileInfo))
        //    {

        //        compileProcess.StandardInput.Write(fullCode);
        //        compileProcess.StandardInput.Close();

        //        compileProcess.WaitForExit();

        //        if (compileProcess.ExitCode != 0)
        //        {
        //            string error = compileProcess.StandardError.ReadToEnd();
        //            return $"Compilation Error:\n{error}";
        //        }
        //    }

        //    ProcessStartInfo executeInfo = new ProcessStartInfo
        //    {
        //        FileName = "./tempCode.exe",
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true
        //    };

        //    using (Process executeProcess = Process.Start(executeInfo))
        //    {
        //        string output = executeProcess.StandardOutput.ReadToEnd();
        //        string error = executeProcess.StandardError.ReadToEnd();

        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            return $"Runtime Error:\n{error}";
        //        }

        //        return output;

        //    }
        //}
        public static string JudgePython(string code)
        {
            // Define the path for the output file
            string outputPath = "output.txt";
            string errorPath = "error.txt";

            // Execute the Python code
            ProcessStartInfo executeInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "-", // Read from standard input
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process executeProcess = Process.Start(executeInfo))
            {
                // Pass the code as standard input to the Python interpreter
                executeProcess.StandardInput.Write(code);
                executeProcess.StandardInput.Close(); // Signal end of input

                string output = executeProcess.StandardOutput.ReadToEnd();
                string error = executeProcess.StandardError.ReadToEnd();

                executeProcess.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return $"Runtime Error:\n{error}";
                }

                return output;
            }
        }
    }
}
