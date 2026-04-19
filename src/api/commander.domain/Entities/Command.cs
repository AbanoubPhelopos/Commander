using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace commander.domain.Entities;

public class Command : ICreatedAtTrackable
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? HowTo { get; set; }
    [Required]
    public string? CommandLine { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public int PlatformId { get; set; }
    [ForeignKey("PlatformId")]
    public Platform? Platform { get; set; }
}
