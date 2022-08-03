using System.Collections.Generic;

namespace DataPiper
{
    public class SqlBulkCopyDestinationOptions : Options
    {
        public string ConnectionString { get; set; }
        public int BulkCopyTimeout { get; set; }
        public string DestinationTableName { get; set; }
        public bool FileHasHeaders { get; set; }
        public IReadOnlyDictionary<int, int> ColumnMappings { get; set; }
    }
}
