using EventPlatform.Api.Modules.Auth;
using EventPlatform.Api.Modules.Events;
using EventPlatform.Api.Modules.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventPlatform.Api.Data;

public class AppDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<AgendaItem> AgendaItems => Set<AgendaItem>();
    public DbSet<Speaker> Speakers => Set<Speaker>();
    public DbSet<TicketType> TicketTypes => Set<TicketType>();
    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Event>(e =>
        {
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Venue).HasMaxLength(200).IsRequired();
        });

        builder.Entity<AgendaItem>()
            .HasOne(x => x.Event)
            .WithMany(x => x.AgendaItems)
            .HasForeignKey(x => x.EventId);

        builder.Entity<Speaker>()
            .HasOne(x => x.Event)
            .WithMany(x => x.Speakers)
            .HasForeignKey(x => x.EventId);

        builder.Entity<TicketType>()
            .HasOne(x => x.Event)
            .WithMany(x => x.TicketTypes)
            .HasForeignKey(x => x.EventId);
    }
}