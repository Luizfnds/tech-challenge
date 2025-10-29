namespace TechChallenge.Application.Common.Exceptions;

/// <summary>
/// Base exception for authentication-related errors
/// </summary>
public abstract class AuthenticationException : Exception
{
    protected AuthenticationException(string message) : base(message)
    {
    }

    protected AuthenticationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when user credentials are invalid
/// </summary>
public class InvalidCredentialsException : AuthenticationException
{
    public InvalidCredentialsException()
        : base("Invalid email or password")
    {
    }

    public InvalidCredentialsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when email is not confirmed
/// </summary>
public class EmailNotConfirmedException : AuthenticationException
{
    public EmailNotConfirmedException()
        : base("Email address is not confirmed")
    {
    }

    public EmailNotConfirmedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when user is not found
/// </summary>
public class UserNotFoundException : AuthenticationException
{
    public UserNotFoundException()
        : base("User not found")
    {
    }

    public UserNotFoundException(string email)
        : base($"User with email '{email}' not found")
    {
    }

    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when user account is disabled
/// </summary>
public class UserDisabledException : AuthenticationException
{
    public UserDisabledException()
        : base("User account is disabled")
    {
    }

    public UserDisabledException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when confirmation code is invalid
/// </summary>
public class InvalidConfirmationCodeException : AuthenticationException
{
    public InvalidConfirmationCodeException()
        : base("Invalid or expired confirmation code")
    {
    }

    public InvalidConfirmationCodeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when password is invalid or doesn't meet requirements
/// </summary>
public class InvalidPasswordException : AuthenticationException
{
    public InvalidPasswordException(string message)
        : base(message)
    {
    }

    public InvalidPasswordException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when user already exists
/// </summary>
public class UserAlreadyExistsException : AuthenticationException
{
    public UserAlreadyExistsException(string email)
        : base($"User with email '{email}' already exists")
    {
    }

    public UserAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when token is invalid or expired
/// </summary>
public class InvalidTokenException : AuthenticationException
{
    public InvalidTokenException()
        : base("Invalid or expired token")
    {
    }

    public InvalidTokenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when password reset operation fails
/// </summary>
public class PasswordResetFailedException : AuthenticationException
{
    public PasswordResetFailedException(string message)
        : base(message)
    {
    }

    public PasswordResetFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when limit is exceeded (e.g., too many attempts)
/// </summary>
public class LimitExceededException : AuthenticationException
{
    public LimitExceededException(string message)
        : base(message)
    {
    }

    public LimitExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
