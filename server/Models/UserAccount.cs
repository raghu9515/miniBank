using System.ComponentModel.DataAnnotations;

namespace miniBank.Api.Models;

public record UserAccount
{
    [Required]
    public required string Id { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    public required string FullName { get; init; }

    public decimal Balance { get; set; }

    public IList<TransactionRecord> Transactions { get; init; } = new List<TransactionRecord>();
}
