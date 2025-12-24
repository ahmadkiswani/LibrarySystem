using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Models;
using LibrarySystem.Logging.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibrarySystem.Logging.Services
{
    public class MongoLogService
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

        public async Task SaveRequestAsync(LogRequestDto dto)
        {
            var log = new HttpLog
            {
                CorrelationId = dto.CorrelationId,
                Time = dto.Time,
                ServiceName = dto.ServiceName,
                Request = dto.Request,
                LogLevel = "Request"
            };

            await _httpLogs.InsertOneAsync(log);
        }

        public async Task SaveResponseAsync(LogResponseDto dto)
        {
            var log = new HttpLog
            {
                CorrelationId = dto.CorrelationId,
                Time = dto.Time,
                ServiceName =dto.ServiceName,
                Response = dto.Response,
                LogLevel = "Response"
            };

            await _httpLogs.InsertOneAsync(log);
        }

        public async Task SaveExceptionAsync(LogExceptionDto dto)
        {
            var log = new ExceptionLog
            {
                CorrelationId = dto.CorrelationId,
                Time = dto.Time,
                ServiceName = dto.ServiceName,
                Message = dto.Message,
                StackTrace = dto.StackTrace,
                Request = dto.Request,
                Response = dto.Response,

            };

            await _exceptionLogs.InsertOneAsync(log);
        }
    }
}
