// CreateCourierDtoValidator - Validator for creating couriers in Application layer
// Validations: unique CNPJ, unique CNH, valid types, valid birth date
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
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must have maximum 100 characters");

        RuleFor(x => x.Cnpj)
            .NotEmpty()
            .WithMessage("CNPJ is required")
            .Length(14)
            .WithMessage("CNPJ must have 14 digits")
            .Matches(@"^\d{14}$")
            .WithMessage("CNPJ must contain only numbers");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Birth date is required")
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage("Courier must be at least 18 years old")
            .GreaterThan(DateTime.Now.AddYears(-100))
            .WithMessage("Birth date cannot be too old");

        RuleFor(x => x.CnhNumber)
            .NotEmpty()
            .WithMessage("CNH number is required")
            .Length(9)
            .WithMessage("CNH number must have 9 digits")
            .Matches(@"^\d{9}$")
            .WithMessage("CNH number must contain only numbers");

        RuleFor(x => x.CnhType)
            .IsInEnum()
            .WithMessage("CNH type must be A, B or AB");

        RuleFor(x => x.CnhImageUrl)
            .NotEmpty()
            .WithMessage("CNH image URL is required")
            .Must(BeValidImageUrl)
            .WithMessage("Image URL must be valid");
    }

    private static bool BeValidImageUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uri) && 
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
