using System.ComponentModel.DataAnnotations;

namespace miniBank.Api.DTOs;

public record RegisterRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [MinLength(6)]
    public required string Password { get; init; }

    [Required]
    [MaxLength(100)]
    public required string FullName { get; init; }
}
