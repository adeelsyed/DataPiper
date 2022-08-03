
namespace DataPiper
{
    public class GnuPGDecryptionTransformerOptions : Options
    {
        //properties
        public string HomeDirectory { get; set; }
        public string PrivateKeyPassphrase { get; set; }
    }
}