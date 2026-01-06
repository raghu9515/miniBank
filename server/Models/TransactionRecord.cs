using System.ComponentModel.DataAnnotations;

namespace miniBank.Api.Models;

public record TransactionRecord
{
    [Required]
    public required string Id { get; init; }

    [Required]
    public required DateTimeOffset Timestamp { get; init; }

    [Required]
    public required decimal Amount { get; init; }

    [Required]
    public required string Description { get; init; }

    [Required]
    public required string Type { get; init; }
}
