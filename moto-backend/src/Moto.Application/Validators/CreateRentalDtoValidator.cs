// CreateRentalDtoValidator - Validator for creating rentals in Application layer
// Validations: courier with category A license, valid plans, required IDs
using FluentValidation;
using Moto.Application.DTOs.Rentals;
using Moto.Domain.Enums;

namespace Moto.Application.Validators;

public class CreateRentalDtoValidator : AbstractValidator<CreateRentalDto>
{
    public CreateRentalDtoValidator()
    {
        RuleFor(x => x.MotorcycleId)
            .NotEmpty()
            .WithMessage("Motorcycle ID is required");

        RuleFor(x => x.CourierId)
            .NotEmpty()
            .WithMessage("Courier ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow.Date)
            .WithMessage("Start date must be after current date");

        RuleFor(x => x.ExpectedEndDate)
            .NotEmpty()
            .WithMessage("Expected end date is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("Expected end date must be after start date")
            .When(x => x.StartDate != default);

        RuleFor(x => x.PlanType)
            .IsInEnum()
            .WithMessage("Plan type must be valid (7, 15, 30, 45 or 50 days)")
            .Must(BeValidPlanType)
            .WithMessage("Plan type must be one of the valid values: 7, 15, 30, 45 or 50 days");
    }

    private static bool BeValidPlanType(RentalPlan planType)
    {
        return planType switch
        {
            RentalPlan.SevenDays => true,
            RentalPlan.FifteenDays => true,
            RentalPlan.ThirtyDays => true,
            RentalPlan.FortyFiveDays => true,
            RentalPlan.FiftyDays => true,
            _ => false
        };
    }
}
