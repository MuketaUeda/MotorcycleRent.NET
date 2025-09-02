// CreateMotorcycleDtoValidator - Validator for creating motorcycles in Application layer
// Validations: unique plate, valid year, required model
using FluentValidation;
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Application.Validators;

public class CreateMotorcycleDtoValidator : AbstractValidator<CreateMotorcycleDto>
{
    public CreateMotorcycleDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("License plate is required")
            .Length(7)
            .WithMessage("License plate must have 7 characters")
            .Matches(@"^[A-Z]{3}[0-9][0-9A-Z][0-9]{2}$")
            .WithMessage("License plate must be in Mercosul format (e.g., ABC1D23)");

        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("Year is required")
            .InclusiveBetween(1900, DateTime.Today.Year + 1)
            .WithMessage($"Year must be between 1900 and {DateTime.Today.Year + 1}");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required")
            .MaximumLength(100)
            .WithMessage("Model must have maximum 100 characters");
    }
}
