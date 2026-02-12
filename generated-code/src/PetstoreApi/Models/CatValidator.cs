
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for Cat model
/// </summary>
public class CatValidator : AbstractValidator<Models.Cat>
{
    public CatValidator()
    {
        RuleFor(x => x.ClassName).NotEmpty();
    }
}


