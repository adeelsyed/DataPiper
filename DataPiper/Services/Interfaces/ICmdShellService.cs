namespace DataPiper
{
    public interface ICmdShellService
    {
        bool ExecCmd(string fileName, string arguments, out int exitCode, out string stdout, out string stderr);
        string FormatCmdOutput(int exitCode, string stdout, string stderr);
    }
}