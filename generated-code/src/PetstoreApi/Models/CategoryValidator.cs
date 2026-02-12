
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for Category model
/// </summary>
public class CategoryValidator : AbstractValidator<Models.Category>
{
    public CategoryValidator()
    {
    }
}


