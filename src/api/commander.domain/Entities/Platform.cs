using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace commander.domain.Entities;

public class Platform
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string PlatformName { get; set; }
}
