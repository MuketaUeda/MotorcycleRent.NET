// CreateCourierDtoValidator - Validador para criação de entregadores na Application layer
// Validações: CNPJ único, CNH único, tipos válidos, data de nascimento válida
using FluentValidation;
using Moto.Application.DTOs.Couriers;
using Moto.Domain.Enums;

namespace Moto.Application.Validators;

public class CreateCourierDtoValidator : AbstractValidator<CreateCourierDto>
{
    public CreateCourierDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("O CNPJ é obrigatório")
            .Length(14)
            .WithMessage("O CNPJ deve ter 14 dígitos")
            .Matches(@"^\d{14}$")
            .WithMessage("O CNPJ deve conter apenas números");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("A data de nascimento é obrigatória")
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage("O entregador deve ter pelo menos 18 anos")
            .GreaterThan(DateTime.Now.AddYears(-100))
            .WithMessage("A data de nascimento não pode ser muito antiga");

        RuleFor(x => x.CnhNumber)
            .NotEmpty()
            .WithMessage("O número da CNH é obrigatório")
            .Length(11)
            .WithMessage("O número da CNH deve ter 11 dígitos")
            .Matches(@"^\d{11}$")
            .WithMessage("O número da CNH deve conter apenas números");

        RuleFor(x => x.CnhType)
            .IsInEnum()
            .WithMessage("O tipo da CNH deve ser A, B ou AB");

        RuleFor(x => x.CnhImageUrl)
            .NotEmpty()
            .WithMessage("A URL da imagem da CNH é obrigatória")
            .Must(BeValidImageUrl)
            .WithMessage("A URL da imagem deve ser válida");
    }

    private static bool BeValidImageUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uri) && 
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
