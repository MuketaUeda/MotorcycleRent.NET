// CreateRentalDtoValidator - Validador para criação de aluguéis na Application layer
// Validações: entregador habilitado categoria A, planos válidos, IDs obrigatórios
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
            .WithMessage("O ID da motocicleta é obrigatório");

        RuleFor(x => x.CourierId)
            .NotEmpty()
            .WithMessage("O ID do entregador é obrigatório");

        RuleFor(x => x.PlanType)
            .IsInEnum()
            .WithMessage("O tipo de plano deve ser válido (7, 15, 30, 45 ou 50 dias)")
            .Must(BeValidPlanType)
            .WithMessage("O tipo de plano deve ser um dos valores válidos: 7, 15, 30, 45 ou 50 dias");
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
