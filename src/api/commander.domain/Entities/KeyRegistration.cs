namespace commander.domain.Entities;

public class KeyRegistration
{
    public int Id { get; set; }

    public Guid KeyIndex { get; set; }

    public Guid Salt { get; set; }

    public required string KeyHash { get; set; }

    public required string Description { get; set; }

    public required string UserId { get; set; }
}
