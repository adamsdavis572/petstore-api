
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for Dog model
/// </summary>
public class DogValidator : AbstractValidator<Models.Dog>
{
    public DogValidator()
    {
        RuleFor(x => x.ClassName).NotEmpty();
    }
}


