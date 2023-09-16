using Catfish.API.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class MySqlMoviesDbContext : DbContext
    {
        public MySqlMoviesDbContext(DbContextOptions<MySqlMoviesDbContext> options)
          : base(options)
        {
        }

        public DbSet<MySqlMovie> Movies { get; set; }
    }

    [Table("movie")]
    public class MySqlMovie
    {
        [Key]
        public int Movie_ID { get; set; }
        public string? Movie_Title { get; set; }
        public int? Parent_ID { get; set; }
        public string? Genre { get; set; }
        public string? Rating { get; set; }
        public string? Director { get; set; }
        public string? Producer { get; set; }
        public string? Actor { get; set; }
        public string? Writer { get; set; }
        public string? Distributor { get; set; }
        public DateTime? Release_Date { get; set; }
        public string? Release_Notes { get; set; }
        public int? Running_Time { get; set; }
        public float? Star_Rating { get; set; }
        public string? URL { get; set; }
    }
}
