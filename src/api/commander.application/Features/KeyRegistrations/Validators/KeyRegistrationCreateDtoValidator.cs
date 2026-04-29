using commander.application.Features.KeyRegistrations.DTOs;
using FluentValidation;

namespace commander.application.Features.KeyRegistrations.Validators;

public class KeyRegistrationCreateDtoValidator : AbstractValidator<KeyRegistrationCreateDto>
{
    public KeyRegistrationCreateDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Description is required");

        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("UserId is required");
    }
}
