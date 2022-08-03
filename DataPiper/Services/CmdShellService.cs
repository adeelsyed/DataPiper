using System.Diagnostics;

namespace DataPiper
{
    public class CmdShellService : ICmdShellService
    {
        public bool ExecCmd(string fileName, string arguments, out int exitCode, out string stdout, out string stderr)
        {
            var process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            stdout = process.StandardOutput.ReadToEnd();
            stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();
            exitCode = process.ExitCode;
            return process.ExitCode == 0; //0 = success
        }
        public string FormatCmdOutput(int exitCode, string stdout, string stderr)
        {
            return $"EXIT CODE:" + exitCode + ". STDOUT: " + stdout + ". STDERR: " + stderr + ".";
        }

    }
}
