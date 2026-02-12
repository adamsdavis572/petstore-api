
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for Pet model
/// </summary>
public class PetValidator : AbstractValidator<Models.Pet>
{
    public PetValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.PhotoUrls).NotNull();
    }
}


