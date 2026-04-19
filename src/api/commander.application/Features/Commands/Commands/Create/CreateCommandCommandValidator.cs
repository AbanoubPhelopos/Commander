using FluentValidation;

namespace commander.application.Features.Commands.Commands.Create;

public class CreateCommandCommandValidator : AbstractValidator<CreateCommandCommand>
{
    public CreateCommandCommandValidator()
    {
        RuleFor(c => c.HowTo).NotEmpty().WithMessage("HowTo is required");
        RuleFor(c => c.CommandLine).NotEmpty().WithMessage("CommandLine is required");
        RuleFor(c => c.PlatformId).NotEmpty().WithMessage("PlatformId is required");
    }
}
