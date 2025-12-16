using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Logging.Settings
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string HttpLogsCollection { get; set; } = null!;
        public string ExceptionLogsCollection { get; set; } = null!;
    }
}

