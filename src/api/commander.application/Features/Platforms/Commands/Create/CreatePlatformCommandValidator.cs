using FluentValidation;

namespace commander.application.Features.Platforms.Commands.Create;

public class CreatePlatformCommandValidator : AbstractValidator<CreatePlatformCommand>
{
    public CreatePlatformCommandValidator()
    {
        RuleFor(x => x.PlatformName)
            .NotEmpty().WithMessage("Platform name is required.")
            .MaximumLength(200).WithMessage("Platform name must not exceed 200 characters.");
    }
}
