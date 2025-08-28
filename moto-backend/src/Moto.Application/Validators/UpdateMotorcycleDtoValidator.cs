// UpdateMotorcycleDtoValidator - Validador para atualização de motos na Application layer
// Validações: apenas placa pode ser alterada, formato válido
using FluentValidation;
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Application.Validators;

public class UpdateMotorcycleDtoValidator : AbstractValidator<UpdateMotorcycleDto>
{
    public UpdateMotorcycleDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("A placa é obrigatória")
            .Length(7, 8)
            .WithMessage("A placa deve ter entre 7 e 8 caracteres")
            .Matches(@"^[A-Z]{3}[0-9][0-9A-Z][0-9]{2}$|^[A-Z]{3}[0-9]{4}$")
            .WithMessage("A placa deve estar no formato válido (ex: ABC1234 ou ABC1D23)");
    }
}
