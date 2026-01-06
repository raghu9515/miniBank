namespace miniBank.Api.Models;

public record AuditEvent
{
    public required string EventType { get; init; }

    public required string Actor { get; init; }

    public required DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;

    public IDictionary<string, object?> Metadata { get; init; } = new Dictionary<string, object?>();
}
