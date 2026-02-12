
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for Order model
/// </summary>
public class OrderValidator : AbstractValidator<Models.Order>
{
    public OrderValidator()
    {
    }
}


