// UpdateCnhImageDtoValidator - Validator for updating CNH image
// Validations: PNG or BMP format, valid URL
using FluentValidation;
using Moto.Application.DTOs.Couriers;

namespace Moto.Application.Validators;

public class UpdateCnhImageDtoValidator : AbstractValidator<UpdateCnhImageDto>
{
    public UpdateCnhImageDtoValidator()
    {
        RuleFor(x => x.CnhImageUrl)
            .NotEmpty()
            .WithMessage("CNH image URL is required")
            .Must(BeValidImageUrl)
            .WithMessage("Image URL must be valid")
            .Must(BeValidImageFormat)
            .WithMessage("Image must be in PNG or BMP format");
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
