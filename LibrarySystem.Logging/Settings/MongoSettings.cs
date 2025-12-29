using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Logging.Settings
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string HttpLogsLibraryCollection { get; set; } = null!;
        public string HttpLogsIdentityCollection { get; set; } = null!;

        public string ExceptionLogsLibraryCollection { get; set; } = null!;
        public string ExceptionLogsIdentityCollection { get; set; } = null!;


    }
}

