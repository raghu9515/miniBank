using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using miniBank.Api.DTOs;
using miniBank.Api.Models;

namespace miniBank.Api.Services;

public interface IAccountService
{
    UserAccount Register(RegisterRequest request);
    string? Login(LoginRequest request);
    UserAccount? GetAccount(string accountId);
    IEnumerable<UserAccount> GetAll();
    TransactionRecord Transfer(TransferRequest request);
}

public class AccountService : IAccountService
{
    private readonly ConcurrentDictionary<string, UserAccount> _accounts = new();
    private readonly ConcurrentDictionary<string, string> _credentials = new();
    private readonly IAuditService _auditService;
    private readonly ITokenService _tokenService;

    public AccountService(IAuditService auditService, ITokenService tokenService)
    {
        _auditService = auditService;
        _tokenService = tokenService;

        SeedSampleData();
    }

    public IEnumerable<UserAccount> GetAll() => _accounts.Values;

    public UserAccount Register(RegisterRequest request)
    {
        if (_accounts.Values.Any(x => x.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Email already registered");
        }

        var account = new UserAccount
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            FullName = request.FullName,
            Balance = 5000m
        };

        _accounts[account.Id] = account;
        _credentials[account.Id] = HashPassword(request.Password);

        _auditService.RecordAsync(new AuditEvent
        {
            EventType = "account.created",
            Actor = account.Email,
            Metadata = new Dictionary<string, object?> { { "accountId", account.Id } }
        });

        return account;
    }

    public string? Login(LoginRequest request)
    {
        var account = _accounts.Values.FirstOrDefault(x => x.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
        if (account is null)
        {
            return null;
        }

        if (!_credentials.TryGetValue(account.Id, out var stored) || !VerifyPassword(request.Password, stored))
        {
            return null;
        }

        _auditService.RecordAsync(new AuditEvent
        {
            EventType = "account.login",
            Actor = account.Email,
            Metadata = new Dictionary<string, object?> { { "accountId", account.Id } }
        });

        return _tokenService.GenerateToken(account.Id, account.Email);
    }

    public UserAccount? GetAccount(string accountId)
    {
        _accounts.TryGetValue(accountId, out var account);
        return account;
    }

    public TransactionRecord Transfer(TransferRequest request)
    {
        if (!_accounts.TryGetValue(request.FromAccountId, out var from))
        {
            throw new InvalidOperationException("Source account not found");
        }

        if (!_accounts.TryGetValue(request.ToAccountId, out var to))
        {
            throw new InvalidOperationException("Destination account not found");
        }

        if (from.Balance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient funds");
        }

        from.Balance -= request.Amount;
        to.Balance += request.Amount;

        var transaction = new TransactionRecord
        {
            Id = Guid.NewGuid().ToString(),
            Timestamp = DateTimeOffset.UtcNow,
            Amount = request.Amount,
            Description = request.Memo ?? $"Transfer to {to.Email}",
            Type = "transfer"
        };

        from.Transactions.Add(transaction);
        to.Transactions.Add(transaction);

        _auditService.RecordAsync(new AuditEvent
        {
            EventType = "account.transfer",
            Actor = from.Email,
            Metadata = new Dictionary<string, object?>
            {
                { "from", from.Id },
                { "to", to.Id },
                { "amount", request.Amount },
                { "memo", request.Memo }
            }
        });

        return transaction;
    }

    public UserAccount? GetAccountByEmail(string email) => _accounts.Values.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private static bool VerifyPassword(string password, string hashed) => HashPassword(password) == hashed;

    private void SeedSampleData()
    {
        var alice = new RegisterRequest
        {
            Email = "alice@example.com",
            Password = "Passw0rd!",
            FullName = "Alice Analyst"
        };

        var bob = new RegisterRequest
        {
            Email = "bob@example.com",
            Password = "Sup3rSecure!",
            FullName = "Bob Banker"
        };

        Register(alice);
        Register(bob);

        Transfer(new TransferRequest
        {
            FromAccountId = GetAccountByEmail(alice.Email)!.Id,
            ToAccountId = GetAccountByEmail(bob.Email)!.Id,
            Amount = 150m,
            Memo = "Welcome bonus"
        });
    }
}
