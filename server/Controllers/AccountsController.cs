using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniBank.Api.DTOs;
using miniBank.Api.Services;

namespace miniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController(IAccountService accounts) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AccountResponse>> GetAll()
    {
        var result = accounts.GetAll().Select(a => new AccountResponse(a.Id, a.Email, a.FullName, a.Balance, a.Transactions));
        return Ok(result);
    }

    [HttpGet("{accountId}")]
    public ActionResult<AccountResponse> GetById(string accountId)
    {
        var account = accounts.GetAccount(accountId);
        if (account is null)
        {
            return NotFound();
        }

        return Ok(new AccountResponse(account.Id, account.Email, account.FullName, account.Balance, account.Transactions));
    }

    [HttpPost("transfer")]
    public ActionResult<TransactionRecord> Transfer([FromBody] TransferRequest request)
    {
        var transaction = accounts.Transfer(request);
        return Ok(transaction);
    }
}
