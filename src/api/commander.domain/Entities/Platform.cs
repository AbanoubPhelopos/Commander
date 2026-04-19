using System.ComponentModel.DataAnnotations;

namespace commander.domain.Entities;

public class Platform : ICreatedAtTrackable
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string PlatformName { get; set; }

    [Required]
    public required DateTime CreatedAt { get; set; }

    public ICollection<Command> Commands { get; } = new List<Command>();
}
