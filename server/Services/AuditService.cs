using System.Text.Json;
using miniBank.Api.Models;

namespace miniBank.Api.Services;

public interface IAuditService
{
    Task RecordAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default);
}

public class AuditService : IAuditService
{
    private readonly string _destination;
    private readonly ILogger<AuditService> _logger;

    public AuditService(IConfiguration configuration, ILogger<AuditService> logger)
    {
        _logger = logger;
        _destination = configuration["Audit:WriteTo"] ?? "logs/audit.log";
        Directory.CreateDirectory(Path.GetDirectoryName(_destination)!);
    }

    public async Task RecordAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(auditEvent);
        await File.AppendAllTextAsync(_destination, payload + Environment.NewLine, cancellationToken);
        _logger.LogInformation("Audit event {EventType} recorded for {Actor}", auditEvent.EventType, auditEvent.Actor);
    }
}
