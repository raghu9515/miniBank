using System.ComponentModel.DataAnnotations;

namespace miniBank.Api.DTOs;

public record TransferRequest
{
    [Required]
    public required string FromAccountId { get; init; }

    [Required]
    public required string ToAccountId { get; init; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; init; }

    [MaxLength(120)]
    public string? Memo { get; init; }
}
