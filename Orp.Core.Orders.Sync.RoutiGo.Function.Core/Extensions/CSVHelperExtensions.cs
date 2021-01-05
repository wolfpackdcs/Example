using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Extensions
{
    public static class CSVHelperExtensions
    {
        private static string _separator = ";";

        public static Stream WriteToCSV<TModel, TClassMap>(this IEnumerable<TModel> records) where TClassMap : ClassMap<TModel>
        {
            Stream stream = new MemoryStream();

            //Ensure that Stream stays open on the TextWriter and the CsvWriter.
            using (TextWriter textWriter = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (CsvWriter csvWriter = new CsvWriter(textWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = _separator,
                HasHeaderRecord = true,
            }))
            {
                csvWriter.Configuration.RegisterClassMap<TClassMap>();

                //Write records.
                if (records?.Any() is true)
                {
                    csvWriter.WriteHeader<TModel>();
                    csvWriter.NextRecord();
                    csvWriter.WriteRecords(records);
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static Stream WriteToCSV<TModel, TClassMap>(this TModel record) where TClassMap : ClassMap<TModel>
            => WriteToCSV<TModel, TClassMap>(new List<TModel>() { record });
    }
}
