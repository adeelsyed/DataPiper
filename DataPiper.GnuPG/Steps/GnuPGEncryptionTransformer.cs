using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    internal class GnuPGEncryptionTransformer : Transformer
    {
        private readonly ICmdShellService _cmdShellService;

        //constructors
        public GnuPGEncryptionTransformer(IServiceProvider svcProvider, ICmdShellService cmdShellService) : base(svcProvider)
        {
            _cmdShellService = cmdShellService;
        }

        //properties
        private new GnuPGEncryptionTransformerOptions Options { get => (GnuPGEncryptionTransformerOptions)base.Options; set => base.Options = value; }

        //methods
        protected override IEnumerable<FileInfo> Transform(IEnumerable<FileInfo> files)
        {
            //gpg must be installed on server and its dir included in PATH environment variable (which should happen automatically upon install). cmd line fyi:
            //encrypt a file: gpg2 --homedir "C:\Users\asyed\AppData\Roaming\gnupg" --recipient "publicstorage_chasepaykeyT@publicstorage.com" --always-trust --output "C:\353020.0000350860.181023.d.B555.dfr_resp.pgp" --encrypt "C:\353020.0000350860.181023.d.B555.dfr_resp"
            //decrypt a file: gpg2 --homedir "C:\Users\asyed\AppData\Roaming\gnupg" --output "C:\353020.0000350860.181023.d.B555.dfr_resp" --batch --passphrase "abc123" --decrypt "C:\353020.0000350860.181023.d.B555.dfr_resp.pgp"

            var encryptedFiles = new List<FileInfo>();

            foreach (var file in files)
            {
                var outputFilePath = Path.Combine(WorkingDirectory, Path.GetFileName(file.FullName)) + ".gpg"; //add .gpg extension

                //build gpg2 command
                File.Delete(outputFilePath); //gpg does not have a good overwrite option
                var gpg2Cmd = $"--homedir \"{Options.HomeDirectory}\" --recipient \"{Options.Recipient}\" --always-trust --output \"{outputFilePath}\" --encrypt \"{file.FullName}\"";
                LogService.LogTrace("        gpg2 command: gpg2 " + gpg2Cmd);

                //run
                var success = _cmdShellService.ExecCmd("gpg2", gpg2Cmd, out int exitCode, out string stdout, out string stderr);
                LogService.LogTrace("        gpg2 output: " + _cmdShellService.FormatCmdOutput(exitCode, stdout, stderr));

                //report
                if (!success)
                    throw new Exception("gpg2 error: " + _cmdShellService.FormatCmdOutput(exitCode, stdout, stderr));

                var outputFile = new FileInfo(outputFilePath);
                encryptedFiles.Add(outputFile);
            }

            return encryptedFiles;
        }
    }
}
