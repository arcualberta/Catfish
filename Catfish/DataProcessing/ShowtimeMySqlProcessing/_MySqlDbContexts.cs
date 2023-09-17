using Catfish.API.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class MySqlCountryOriginDbContext : DbContext
    {
        public MySqlCountryOriginDbContext(DbContextOptions<MySqlCountryOriginDbContext> options) : base(options) { }
        public DbSet<MySqlCountryOrigin> Data { get; set; }
    }

    public class MySqlDistributionDbContext : DbContext
    {
        public MySqlDistributionDbContext(DbContextOptions<MySqlDistributionDbContext> options) : base(options) { }
        public DbSet<MySqlDistribution> Data { get; set; }
    }

    public class MySqlMoviesDbContext : DbContext
    {
        public MySqlMoviesDbContext(DbContextOptions<MySqlMoviesDbContext> options) : base(options) { }
        public DbSet<MySqlMovie> Data { get; set; }
    }

    public class MySqlMovieCastDbContext : DbContext
    {
        public MySqlMovieCastDbContext(DbContextOptions<MySqlMovieCastDbContext> options) : base(options) { }
        public DbSet<MySqlMovieCast> Data { get; set; }
    }

    public class MySqlMovieGenreDbContext : DbContext
    {
        public MySqlMovieGenreDbContext(DbContextOptions<MySqlMovieGenreDbContext> options) : base(options) { }
        public DbSet<MySqlMovieGenre> Data { get; set; }
    }
    
    public class MySqlTheaterDbContext : DbContext
    {
        public MySqlTheaterDbContext(DbContextOptions<MySqlTheaterDbContext> options) : base(options) { }
        public DbSet<MySqlTheater> Data { get; set; }
    }
}
