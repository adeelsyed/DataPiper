namespace DataPiper
{
    public class SqlClientSourceOptions : Options
    {
        public string ConnectionString { get; set; }
        public string CommandText { get; set; }
        public string CommandType { get; set; }
        public int CommandTimeout { get; set; }
        public string ColumnDelimiter { get; set; }
    }
}
