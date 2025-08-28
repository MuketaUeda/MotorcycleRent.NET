// CreateMotorcycleDtoValidator - Validador para criação de motos na Application layer
// Validações: placa única, ano válido, modelo obrigatório
using FluentValidation;
using Moto.Application.DTOs.Motorcycles;

namespace Moto.Application.Validators;

public class CreateMotorcycleDtoValidator : AbstractValidator<CreateMotorcycleDto>
{
    public CreateMotorcycleDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("A placa é obrigatória")
            .Length(7, 8)
            .WithMessage("A placa deve ter entre 7 e 8 caracteres")
            .Matches(@"^[A-Z]{3}[0-9][0-9A-Z][0-9]{2}$|^[A-Z]{3}[0-9]{4}$")
            .WithMessage("A placa deve estar no formato válido (ex: ABC1234 ou ABC1D23)");

        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("O ano é obrigatório")
            .InclusiveBetween(1900, DateTime.Now.Year + 1)
            .WithMessage($"O ano deve estar entre 1900 e {DateTime.Now.Year + 1}");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("O modelo é obrigatório")
            .MaximumLength(100)
            .WithMessage("O modelo deve ter no máximo 100 caracteres");
    }
}
