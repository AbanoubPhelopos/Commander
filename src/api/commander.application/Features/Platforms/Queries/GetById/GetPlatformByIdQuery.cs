using commander.application.Features.Platforms.DTOs;
using MediatR;

namespace commander.application.Features.Platforms.Queries.GetById;

public record GetPlatformByIdQuery(int Id) : IRequest<PlatformDto?>;
