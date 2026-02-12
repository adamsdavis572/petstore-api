
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for User model
/// </summary>
public class UserValidator : AbstractValidator<Models.User>
{
    public UserValidator()
    {
    }
}


