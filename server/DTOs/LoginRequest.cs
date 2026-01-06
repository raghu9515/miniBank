using System.ComponentModel.DataAnnotations;

namespace miniBank.Api.DTOs;

public record LoginRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}
