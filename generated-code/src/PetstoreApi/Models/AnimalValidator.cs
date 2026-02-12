
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for Animal model
/// </summary>
public class AnimalValidator : AbstractValidator<Models.Animal>
{
    public AnimalValidator()
    {
        RuleFor(x => x.ClassName).NotEmpty();
    }
}


