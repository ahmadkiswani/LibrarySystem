using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Interfaces;
using LibrarySystem.Logging.Models;
using LibrarySystem.Logging.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibrarySystem.Logging.Services
{
    public class MongoLogService : ILogService
    {
        private readonly IMongoCollection<HttpLog> _httpLogs;
        private readonly IMongoCollection<ExceptionLog> _exceptionLogs;

        public MongoLogService(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);

            _httpLogs = db.GetCollection<HttpLog>(settings.Value.HttpLogsCollection);
            _exceptionLogs = db.GetCollection<ExceptionLog>(settings.Value.ExceptionLogsCollection);
        }

        public async Task LogRequestAsync(LogRequestDto dto)
        {
            var log = new HttpLog
            {
                CorrelationId = dto.CorrelationId,
                Time = DateTime.UtcNow,
                ServiceName = dto.ServiceName,
                Request = dto.Request,
                LogLevel = "Request"
            };

            await _httpLogs.InsertOneAsync(log);
        }

        public async Task LogResponseAsync(LogResponseDto dto)
        {
            var log = new HttpLog
            {
                CorrelationId = dto.CorrelationId,
                Time = DateTime.UtcNow,
                ServiceName = dto.ServiceName,
                Response = dto.Response,
                LogLevel = "Response"
            };

            await _httpLogs.InsertOneAsync(log);
        }

        public async Task LogExceptionAsync(LogExceptionDto dto)
        {
            var log = new ExceptionLog
            {
                CorrelationId = dto.CorrelationId,
                Time = DateTime.UtcNow,
                ServiceName = dto.ServiceName,
                ExceptionMessage = dto.Message,
                StackTrace = dto.StackTrace
            };

            await _exceptionLogs.InsertOneAsync(log);
        }
    }
}
