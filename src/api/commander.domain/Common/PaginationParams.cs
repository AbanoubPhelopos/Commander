using System.ComponentModel.DataAnnotations;

namespace commander.domain.Common;

public record PaginationParams
{
    [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be at least 1.")]
    public int PageIndex { get; init; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; init; } = 10;
}
