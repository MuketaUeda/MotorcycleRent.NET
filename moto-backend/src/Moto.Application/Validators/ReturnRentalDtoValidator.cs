// ReturnRentalDtoValidator - Validador para finalização de aluguel
// Validações: data de retorno obrigatória, não pode ser anterior à data de início
using FluentValidation;
using Moto.Application.DTOs.Rentals;

namespace Moto.Application.Validators;

public class ReturnRentalDtoValidator : AbstractValidator<ReturnRentalDto>
{
    public ReturnRentalDtoValidator()
    {
        RuleFor(x => x.ReturnDate)
            .NotEmpty()
            .WithMessage("A data de retorno é obrigatória")
            .GreaterThan(DateTime.Now)
            .WithMessage("A data de retorno deve ser futura");
    }
}
