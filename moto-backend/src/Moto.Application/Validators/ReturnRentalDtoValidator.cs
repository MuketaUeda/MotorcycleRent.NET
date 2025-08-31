// ReturnRentalDtoValidator - Validator for finalizing a rental
// Validations: return date required, cannot be in the past
using FluentValidation;
using Moto.Application.DTOs.Rentals;

namespace Moto.Application.Validators;

public class ReturnRentalDtoValidator : AbstractValidator<ReturnRentalDto>
{
    public ReturnRentalDtoValidator()
    {
        RuleFor(x => x.ReturnDate)
            .NotEmpty()
            .WithMessage("Return date is required")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Return date must be today or in the future");
    }
}
