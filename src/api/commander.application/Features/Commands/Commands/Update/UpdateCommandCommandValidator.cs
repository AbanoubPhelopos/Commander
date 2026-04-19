using FluentValidation;

namespace commander.application.Features.Commands.Commands.Update;

public class UpdateCommandCommandValidator : AbstractValidator<UpdateCommandCommand>
{
    public UpdateCommandCommandValidator()
    {
        RuleFor(c => c.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        RuleFor(c => c.HowTo).NotEmpty().WithMessage("HowTo is required");
        RuleFor(c => c.CommandLine).NotEmpty().WithMessage("CommandLine is required");
        RuleFor(c => c.PlatformId).GreaterThan(0).WithMessage("PlatformId must be greater than 0");
    }
}