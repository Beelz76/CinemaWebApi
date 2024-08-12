using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccessLayer
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => 
            {
                entity.HasKey(x => x.UserId);
                entity.HasMany(x => x.Tickets).WithOne(x => x.User).HasForeignKey("UserId"); 
            });
            
            builder.Entity<Movie>(entity =>
            {
                entity.HasKey(x => x.MovieId);
                entity.HasMany(x => x.Screenings).WithOne(x => x.Movie).HasForeignKey("MovieId");
            });

            builder.Entity<Director>(entity =>
            {
                entity.HasKey(x => x.DirectorId);
                entity.HasMany(x => x.Movies).WithMany(x => x.Directors)
                    .UsingEntity("MovieDirector",
                        l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.MovieId)),
                        r => r.HasOne(typeof(Director)).WithMany().HasForeignKey("DirectorId").HasPrincipalKey(nameof(Director.DirectorId)),
                        j => j.HasKey("DirectorId", "MovieId"));
            });

            builder.Entity<Country>(entity =>
            {
                entity.HasKey(x => x.CountryId);
                entity.HasMany(x => x.Movies).WithMany(x => x.Countries)
                    .UsingEntity("MovieCountry",
                        l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.MovieId)),
                        r => r.HasOne(typeof(Country)).WithMany().HasForeignKey("CountryId").HasPrincipalKey(nameof(Country.CountryId)),
                        j => j.HasKey("CountryId", "MovieId"));
            });
            
            builder.Entity<Genre>(entity =>
            {
                entity.HasKey(x => x.GenreId);
                entity.HasMany(x => x.Movies).WithMany(x => x.Genres)
                    .UsingEntity("MovieGenre",
                        l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.MovieId)),
                        r => r.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId").HasPrincipalKey(nameof(Genre.GenreId)),
                        j => j.HasKey("GenreId", "MovieId"));
            });
            
            builder.Entity<Seat>(entity =>
            {
                entity.HasKey(x => x.SeatId);
                entity.HasMany(x => x.Tickets).WithOne(x => x.Seat).HasForeignKey("SeatId");
                entity.HasOne(x => x.Hall).WithMany(x => x.Seats).HasForeignKey("HallId");
            });

            builder.Entity<Hall>(entity =>
            {
                entity.HasKey(x => x.HallId);
                entity.HasMany(x => x.Screenings).WithOne(x => x.Hall).HasForeignKey("HallId");
                entity.HasMany(x => x.Seats).WithOne(x => x.Hall).HasForeignKey("HallId");
            });
            
            builder.Entity<Screening>(entity =>
            {
                entity.HasKey(x => x.ScreeningId);
                entity.HasMany(x => x.Tickets).WithOne(x => x.Screening).HasForeignKey("ScreeningId");
                entity.HasOne(x => x.ScreeningPrice).WithMany(x => x.Screenings).HasForeignKey("ScreeningPriceId");
                entity.HasOne(x => x.Movie).WithMany(x => x.Screenings).HasForeignKey("MovieId");
                entity.HasOne(x => x.Hall).WithMany(x => x.Screenings).HasForeignKey("HallId");
            });

            builder.Entity<ScreeningPrice>(entity =>
            {
                entity.HasKey(x => x.ScreeningPriceId);
                entity.HasMany(x => x.Screenings).WithOne(x => x.ScreeningPrice).HasForeignKey("ScreeningPriceId");
            });
            
            builder.Entity<Ticket>(entity =>
            {
                entity.HasKey(x => x.TicketId);
                entity.HasOne(x => x.User).WithMany(x => x.Tickets).HasForeignKey("UserId");
                entity.HasOne(x => x.Screening).WithMany(x => x.Tickets).HasForeignKey("ScreeningId");
                entity.HasOne(x => x.Seat).WithMany(x => x.Tickets).HasForeignKey("SeatId");
            });
        }
    }
}
