using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Models;
using MongoDB.Driver;

namespace LibrarySystem.Logging.Services
{
    public class MongoLogService
    {
        private readonly IMongoCollection<HttpLog> _httpLogsLibrary;
        private readonly IMongoCollection<HttpLog> _httpLogsIdentity;

        private readonly IMongoCollection<ExceptionLog> _exceptionLogsLibrary;
        private readonly IMongoCollection<ExceptionLog> _exceptionLogsIdentity;

        public MongoLogService(IMongoDatabase db)
        {
            _httpLogsLibrary = db.GetCollection<HttpLog>("http_logs_library");
            _httpLogsIdentity = db.GetCollection<HttpLog>("http_logs_identity");

            _exceptionLogsLibrary = db.GetCollection<ExceptionLog>("exception_logs_library");
            _exceptionLogsIdentity = db.GetCollection<ExceptionLog>("exception_logs_identity");
        }

        private bool IsIdentity(string? serviceName)
            => string.Equals(serviceName, "UserIdentity.API", StringComparison.OrdinalIgnoreCase);

        public async Task SaveRequestAsync(LogRequestDto dto)
        {
            var log = new HttpLog
            {
                CorrelationId = dto.CorrelationId,
                Time = dto.Time,
                ServiceName = dto.ServiceName ?? string.Empty,
                Request = dto.Request ?? string.Empty,
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
                ServiceName = dto.ServiceName ?? string.Empty,
                Response = dto.Response ?? string.Empty,
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
                Response = dto.Response
            };

            if (IsIdentity(dto.ServiceName))
                await _exceptionLogsIdentity.InsertOneAsync(log);
            else
                await _exceptionLogsLibrary.InsertOneAsync(log);
        }
    }
}
