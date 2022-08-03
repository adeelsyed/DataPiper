namespace DataPiper
{
    public class SftpDestinationOptions : Options
    {
		public string Host { get; set; }
		public string RemotePath { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string HostKeyFingerprint { get; set; }
		public string PrivateKeyPath { get; set; }
		public int? ConnectionTimeout { get; set; }
	}
}
