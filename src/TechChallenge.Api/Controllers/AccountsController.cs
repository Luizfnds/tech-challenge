using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TechChallenge.Application.Commands.Auth.SignUp;
using TechChallenge.Application.Commands.Auth.ConfirmSignUp;
using TechChallenge.Application.Commands.Auth.ResendConfirmationCode;
using TechChallenge.Application.Commands.Auth.SignIn;
using TechChallenge.Application.Commands.Auth.ForgotPassword;
using TechChallenge.Application.Commands.Auth.ResetPassword;
using TechChallenge.Application.Commands.Auth.ChangePassword;
using TechChallenge.Application.Commands.Auth.EnableUser;
using TechChallenge.Application.Commands.Auth.DisableUser;

namespace TechChallenge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IMediator mediator, ILogger<AccountsController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<AccountsController> _logger = logger;

    [HttpPost("signup")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(SignUp),
            new { id = result },
            new { userId = result, message = "User registered successfully. Please check your email to confirm your account." });
    }

    [HttpPost("confirm-signup")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmSignUp([FromBody] ConfirmSignUpCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Email confirmed successfully. You can now sign in." });
    }

    [HttpPost("resend-confirmation-code")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResendConfirmationCode([FromBody] ResendConfirmationCodeCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Confirmation code resent successfully. Please check your email." });
    }

    [HttpPost("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Password reset code sent to your email." });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Password reset successfully. You can now sign in with your new password." });
    }

    [HttpPost("change-password")]
    [Authorize(Policy = "UserOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Password changed successfully." });
    }

    [HttpPost("enable-user")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> EnableUser([FromBody] EnableUserCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "User enabled successfully." });
    }

    [HttpPost("disable-user")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DisableUser([FromBody] DisableUserCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "User disabled successfully." });
    }
}
