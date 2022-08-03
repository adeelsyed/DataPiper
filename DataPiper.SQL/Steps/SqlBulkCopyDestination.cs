using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace DataPiper
{
    internal class SqlBulkCopyDestination : Destination
    {
        //constructors
        public SqlBulkCopyDestination(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new SqlBulkCopyDestinationOptions Options { get => (SqlBulkCopyDestinationOptions)base.Options; set => base.Options = value; }

        //methods
        protected override void Load(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                using (var csvRdr = new CsvReader(new StreamReader(file.FullName), Options.FileHasHeaders)) //assumes input file is a csv; could add config entry and branch
                using (var copier = new SqlBulkCopy(Options.ConnectionString))
                {
                    copier.DestinationTableName = Options.DestinationTableName;
                    copier.BulkCopyTimeout = Options.BulkCopyTimeout;

                    for (int i = 0; i < csvRdr.FieldCount; i++)
                    {
                        //add column for every csv field
                        csvRdr.Columns.Add(new Column());
                        //but map only if mapped in config
                        if (Options.ColumnMappings.ContainsKey(i))
                            copier.ColumnMappings.Add(i, Options.ColumnMappings[i]);
                    }

                    try
                    {
                        LogService.LogDebug($"Bulk copying {file} into {Options.DestinationTableName}");
                        copier.WriteToServer(csvRdr);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new Exception($"There was a problem loading {file} into {Options.DestinationTableName}. Please verify the column order, datatypes, and nullability on the DB match the CSV. The CSV field count is {csvRdr.FieldCount}.", ex);
                    }
                }
                file.Delete();
            }
        }
    }
}
