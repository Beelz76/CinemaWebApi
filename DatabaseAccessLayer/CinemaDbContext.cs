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
            builder.Entity<User>().HasKey(x => x.UserId);
            builder.Entity<User>().HasMany(x => x.Tickets).WithOne(x => x.User).HasForeignKey("UserId");

            builder.Entity<Movie>().HasKey(x => x.MovieId);
            builder.Entity<Movie>().HasMany(x => x.Screenings).WithOne(x => x.Movie).HasForeignKey("MovieId");

            builder.Entity<Director>().HasKey(x => x.DirectorId);
            builder.Entity<Director>().HasMany(x => x.Movies).WithMany(x => x.Directors)
                .UsingEntity("MovieDirector",
                    l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.MovieId)),
                    r => r.HasOne(typeof(Director)).WithMany().HasForeignKey("DirectorId").HasPrincipalKey(nameof(Director.DirectorId)),
                    j => j.HasKey("DirectorId", "MovieId"));

            builder.Entity<Country>().HasKey(x => x.CountryId);
            builder.Entity<Country>().HasMany(x => x.Movies).WithMany(x => x.Countries)
                .UsingEntity("MovieCountry",
                    l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.MovieId)),
                    r => r.HasOne(typeof(Country)).WithMany().HasForeignKey("CountryId").HasPrincipalKey(nameof(Country.CountryId)),
                    j => j.HasKey("CountryId", "MovieId"));

            builder.Entity<Genre>().HasKey(x => x.GenreId);
            builder.Entity<Genre>().HasMany(x => x.Movies).WithMany(x => x.Genres)
                .UsingEntity("MovieGenre",
                l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.MovieId)),
                r => r.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId").HasPrincipalKey(nameof(Genre.GenreId)),
                j => j.HasKey("GenreId", "MovieId"));

            builder.Entity<Seat>().HasKey(x => x.SeatId);
            builder.Entity<Seat>().HasMany(x => x.Tickets).WithOne(x => x.Seat).HasForeignKey("SeatId");
            builder.Entity<Seat>().HasOne(x => x.Hall).WithMany(x => x.Seats).HasForeignKey("HallId");

            builder.Entity<Hall>().HasKey(x => x.HallId);
            builder.Entity<Hall>().HasMany(x => x.Screenings).WithOne(x => x.Hall).HasForeignKey("HallId");
            builder.Entity<Hall>().HasMany(x => x.Seats).WithOne(x => x.Hall).HasForeignKey("HallId");

            builder.Entity<Screening>().HasKey(x => x.ScreeningId);
            builder.Entity<Screening>().HasMany(x => x.Tickets).WithOne(x => x.Screening).HasForeignKey("ScreeningId");
            builder.Entity<Screening>().HasOne(x => x.ScreeningPrice).WithMany(x => x.Screenings).HasForeignKey("ScreeningPriceId");
            builder.Entity<Screening>().HasOne(x => x.Movie).WithMany(x => x.Screenings).HasForeignKey("MovieId");
            builder.Entity<Screening>().HasOne(x => x.Hall).WithMany(x => x.Screenings).HasForeignKey("HallId");

            builder.Entity<ScreeningPrice>().HasKey(x => x.ScreeningPriceId);
            builder.Entity<ScreeningPrice>().HasMany(x => x.Screenings).WithOne(x => x.ScreeningPrice).HasForeignKey("ScreeningPriceId");

            builder.Entity<Ticket>().HasKey(x => x.TicketId);
            builder.Entity<Ticket>().HasOne(x => x.User).WithMany(x => x.Tickets).HasForeignKey("UserId");
            builder.Entity<Ticket>().HasOne(x => x.Screening).WithMany(x => x.Tickets).HasForeignKey("ScreeningId");
            builder.Entity<Ticket>().HasOne(x => x.Seat).WithMany(x => x.Tickets).HasForeignKey("SeatId");
        }
    }
}
