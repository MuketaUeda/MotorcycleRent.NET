using FluentValidation;
using Moto.Domain.Entities;

namespace Moto.Application.Validators;

public class MotorcycleEventValidator : AbstractValidator<MotorcycleEvent>
{
    public MotorcycleEventValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.MotorcycleId)
            .NotEmpty()
            .WithMessage("MotorcycleId is required");

        RuleFor(x => x.EventType)
            .NotEmpty()
            .WithMessage("EventType is required")
            .MaximumLength(50)
            .WithMessage("EventType cannot exceed 50 characters");

        RuleFor(x => x.EventDate)
            .NotEmpty()
            .WithMessage("EventDate is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("EventDate cannot be in the future");

        RuleFor(x => x.MotorcycleYear)
            .GreaterThan(1900)
            .WithMessage("MotorcycleYear must be greater than 1900")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("MotorcycleYear cannot be more than next year");

        RuleFor(x => x.MotorcycleModel)
            .NotEmpty()
            .WithMessage("MotorcycleModel is required")
            .MaximumLength(100)
            .WithMessage("MotorcycleModel cannot exceed 100 characters");

        RuleFor(x => x.MotorcyclePlate)
            .NotEmpty()
            .WithMessage("MotorcyclePlate is required")
            .MaximumLength(50)
            .WithMessage("MotorcyclePlate cannot exceed 50 characters")
            .Matches(@"^[A-Z]{3}[0-9][0-9A-Z][0-9]{2}$")
            .WithMessage("MotorcyclePlate must be in Brazilian format (e.g., ABC1234 or ABC1D23)");

        RuleFor(x => x.AdditionalData)
            .MaximumLength(1000)
            .WithMessage("AdditionalData cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.AdditionalData));
    }
}
