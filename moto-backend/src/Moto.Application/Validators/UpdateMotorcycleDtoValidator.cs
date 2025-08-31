// UpdateMotorcycleDtoValidator - Validator for updating motorcycles in Application layer
// Validations: only plate can be changed, valid format
using FluentValidation;
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Application.Validators;

public class UpdateMotorcycleDtoValidator : AbstractValidator<UpdateMotorcycleDto>
{
    public UpdateMotorcycleDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("License plate is required")
            .Length(7)
            .WithMessage("License plate must have 7 characters")
            .Matches(@"^[A-Z]{3}[0-9][0-9A-Z][0-9]{2}$")
            .WithMessage("License plate must be in Mercosul format (e.g., ABC1D23)");
    }
}
