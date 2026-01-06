using miniBank.Api.Models;

namespace miniBank.Api.DTOs;

public record AccountResponse(string Id, string Email, string FullName, decimal Balance, IEnumerable<TransactionRecord> Transactions);
