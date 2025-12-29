using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Models;
using LibrarySystem.Logging.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibrarySystem.Logging.Services
{
    public class MongoLogService
    {
        private readonly IMongoCollection<HttpLog> _httpLogsLibrary;
        private readonly IMongoCollection<HttpLog> _httpLogsIdentity;

        private readonly IMongoCollection<ExceptionLog> _exceptionLogsLibrary;
        private readonly IMongoCollection<ExceptionLog> _exceptionLogsIdentity;

        public MongoLogService(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);

            _httpLogsLibrary = db.GetCollection<HttpLog>(settings.Value.HttpLogsLibraryCollection);
            _httpLogsIdentity = db.GetCollection<HttpLog>(settings.Value.HttpLogsIdentityCollection);

            _exceptionLogsLibrary = db.GetCollection<ExceptionLog>(settings.Value.ExceptionLogsLibraryCollection);
            _exceptionLogsIdentity = db.GetCollection<ExceptionLog>(settings.Value.ExceptionLogsIdentityCollection);
        }

        private bool IsIdentity(string? serviceName)
            => string.Equals(serviceName, "UserIdentity.API", StringComparison.OrdinalIgnoreCase);

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

            if (IsIdentity(dto.ServiceName))
                await _httpLogsIdentity.InsertOneAsync(log);
            else
                await _httpLogsLibrary.InsertOneAsync(log);
        }

        public async Task SaveResponseAsync(LogResponseDto dto)
        {
            var log = new HttpLog
            {
                CorrelationId = dto.CorrelationId,
                Time = dto.Time,
                ServiceName = dto.ServiceName,
                Response = dto.Response,
                LogLevel = "Response"
            };

            if (IsIdentity(dto.ServiceName))
                await _httpLogsIdentity.InsertOneAsync(log);
            else
                await _httpLogsLibrary.InsertOneAsync(log);
        }

        public async Task SaveExceptionAsync(LogExceptionDto dto)
        {
            var log = new ExceptionLog
            {
                CorrelationId = dto.CorrelationId,
                Time = dto.Time,
                ServiceName = dto.ServiceName,
                Message = dto.Message,
                Request = dto.Request,
                Response = dto.Response,
            };

            if (IsIdentity(dto.ServiceName))
                await _exceptionLogsIdentity.InsertOneAsync(log);
            else
                await _exceptionLogsLibrary.InsertOneAsync(log);
        }

    }
}
