using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DataPiper
{
    internal class SqlClientSource : Source
    {
        //constructors
        public SqlClientSource(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new SqlClientSourceOptions Options { get => (SqlClientSourceOptions)base.Options; set => base.Options = value; }

        //methods
        protected override IEnumerable<FileInfo> Extract()
        {
            var extractedFiles = new List<FileInfo>();

            using (var con = new SqlConnection(Options.ConnectionString))
            using (var cmd = new SqlCommand(Options.CommandText, con))
            {
                cmd.CommandType = Enum.Parse<CommandType>(Options.CommandType);
                cmd.CommandTimeout = Options.CommandTimeout;

                LogService.LogDebug("Connecting to DB");
                con.Open();
                LogService.LogDebug("Executing command");
                using (var rdr = cmd.ExecuteReader())
                {
                    do
                    {
                        if (rdr.HasRows)
                        {
                            //create a file
                            var localPath = Path.Combine(WorkingDirectory, Guid.NewGuid().ToString());
                            LogService.LogDebug("Creating file " + localPath);
                            using (var file = File.CreateText(localPath))
                            {
                                //and write lines one by one
                                object[] values = new object[rdr.FieldCount];
                                while (rdr.Read())
                                {
                                    rdr.GetValues(values);
                                    file.WriteLine(string.Join(Options.ColumnDelimiter, values));
                                }
                            }

                            //add file to collection
                            var downloadedFile = new FileInfo(localPath);
                            extractedFiles.Add(downloadedFile);
                        }
                        else
                            LogService.LogDebug("No records");
                    }
                    while (rdr.NextResult());
                }
            }

            return extractedFiles;
        }
    }
}
