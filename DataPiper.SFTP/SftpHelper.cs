using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataPiper
{
    internal static class SftpHelper
    {
        public static ConnectionInfo GetConnectionInfo(string host, string userName, string password, string privateKeyPath)
        {
            PasswordAuthenticationMethod passwordMethod = null;
            PrivateKeyAuthenticationMethod keyMethod = null;

            if (!string.IsNullOrEmpty(password))
                passwordMethod = new PasswordAuthenticationMethod(userName, password);
            if (!string.IsNullOrWhiteSpace(privateKeyPath))
                keyMethod = new PrivateKeyAuthenticationMethod(userName, new PrivateKeyFile(privateKeyPath));

            if (passwordMethod != null && keyMethod != null)
                return new ConnectionInfo(host, userName, passwordMethod, keyMethod);
            if (passwordMethod != null)
                return new ConnectionInfo(host, userName, passwordMethod);
            if (keyMethod != null)
                return new ConnectionInfo(host, userName, keyMethod);

            throw new Exception("Must supply password, private key, or both to authenticate");
        }
        public static void ValidateHostKey(string hostKeyFingerprint, HostKeyEventArgs e)
        {
            //convert each byte to hex and colon delimit
            var received = string.Join(":", e.FingerPrint.Select(b => b.ToString("X2"))).ToLower();
            //logger.Debug($"Received HostKeyFingerprint: {e.HostKeyName} {e.KeyLength} {received}");

            //find colon delimited hex in configured fingerprint
            var trusted = Regex.Match(hostKeyFingerprint.ToLower(), "([0-9a-f]{2}:){15}[0-9a-f]{2}").Value;

            //if they don't match, it may be the server has more than one fingerprint, based on different host key algorithms
            //(ssh-ed25519, ecdsa-sha2-nistp256, ecdsa-sha2-nistp384, ecdsa-sha2-nistp521, ssh-rsa, ssh-dss, etc.)
            //the config file should have the MD5-hashed fingerprint that matches the algorithm specified in e.HostKeyName
            e.CanTrust = received == trusted;
        }
    }
}
