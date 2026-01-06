using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniBank.Api.DTOs;
using miniBank.Api.Services;

namespace miniBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accounts;

    public AuthController(IAccountService accounts)
    {
        _accounts = accounts;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public ActionResult<AccountResponse> Register([FromBody] RegisterRequest request)
    {
        var account = _accounts.Register(request);
        return Created($"/api/accounts/{account.Id}", new AccountResponse(account.Id, account.Email, account.FullName, account.Balance, account.Transactions));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        var token = _accounts.Login(request);
        if (token is null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new { token });
    }
}
