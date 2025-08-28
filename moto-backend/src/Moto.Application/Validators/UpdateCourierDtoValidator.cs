// UpdateCourierDtoValidator - Validador para atualização de entregadores na Application layer
// Validações: apenas nome e imagem da CNH podem ser alterados
using FluentValidation;
using Moto.Application.DTOs.Couriers;

namespace Moto.Application.Validators;

public class UpdateCourierDtoValidator : AbstractValidator<UpdateCourierDto>
{
    public UpdateCourierDtoValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .WithMessage("O nome deve ter no máximo 100 caracteres");
        });

        When(x => !string.IsNullOrEmpty(x.CnhImageUrl), () =>
        {
            RuleFor(x => x.CnhImageUrl)
                .Must(BeValidImageUrl)
                .WithMessage("A URL da imagem deve ser válida");
        });
    }

    private static bool BeValidImageUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uri) && 
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
