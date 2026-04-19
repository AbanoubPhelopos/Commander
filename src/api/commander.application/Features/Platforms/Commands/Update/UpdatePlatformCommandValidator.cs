using FluentValidation;

namespace commander.application.Features.Platforms.Commands.Update;

public class UpdatePlatformCommandValidator : AbstractValidator<UpdatePlatformCommand>
{
    public UpdatePlatformCommandValidator()
    {
        RuleFor(x => x.PlatformName)
            .NotEmpty().WithMessage("Platform name is required.")
            .MaximumLength(200).WithMessage("Platform name must not exceed 200 characters.");

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Platform ID must be greater than 0.");
    }
}
