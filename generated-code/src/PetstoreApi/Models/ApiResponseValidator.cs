
using FluentValidation;

namespace PetstoreApi.Validators;

/// <summary>
/// Validator for ApiResponse model
/// </summary>
public class ApiResponseValidator : AbstractValidator<Models.ApiResponse>
{
    public ApiResponseValidator()
    {
    }
}


