// MotorcycleCreatedEventDtoValidator - Validator for motorcycle created events
// Validations: required fields, valid formats, reasonable ranges
using FluentValidation;
using Moto.Application.DTOs.Events;

namespace Moto.Application.Validators;

public class MotorcycleCreatedEventDtoValidator : AbstractValidator<MotorcycleCreatedEventDto>
{
    public MotorcycleCreatedEventDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required")
            .MaximumLength(100)
            .WithMessage("Model cannot exceed 100 characters");

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Plate is required")
            .MaximumLength(50)
            .WithMessage("Plate cannot exceed 50 characters")
            .Matches(@"^[A-Z]{3}[0-9][0-9A-Z][0-9]{2}$")
            .WithMessage("Plate must be in Mercosul format (e.g., ABC1D23)");

        RuleFor(x => x.Year)
            .GreaterThan(1900)
            .WithMessage("Year must be greater than 1900")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("Year cannot be more than next year");
    }
}
