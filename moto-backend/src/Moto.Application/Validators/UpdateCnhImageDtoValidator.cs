// UpdateCnhImageDtoValidator - Validador para atualização da imagem da CNH
// Validações: formato PNG ou BMP, URL válida
using FluentValidation;

namespace Moto.Application.Validators;

public class UpdateCnhImageDtoValidator : AbstractValidator<UpdateCnhImageDto>
{
    public UpdateCnhImageDtoValidator()
    {
        RuleFor(x => x.CnhImageUrl)
            .NotEmpty()
            .WithMessage("A URL da imagem da CNH é obrigatória")
            .Must(BeValidImageUrl)
            .WithMessage("A URL da imagem deve ser válida")
            .Must(BeValidImageFormat)
            .WithMessage("A imagem deve estar no formato PNG ou BMP");
    }

    private static bool BeValidImageUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uri) && 
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeValidImageFormat(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        var lowerUrl = url.ToLowerInvariant();
        return lowerUrl.EndsWith(".png") || lowerUrl.EndsWith(".bmp");
    }
}

// DTO para atualização da imagem da CNH
public class UpdateCnhImageDto
{
    public required string CnhImageUrl { get; set; }
}
