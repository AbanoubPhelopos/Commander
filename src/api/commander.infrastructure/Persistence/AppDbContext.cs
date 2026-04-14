using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commander.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace commander.infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Platform> Platforms { get; set; }
}
