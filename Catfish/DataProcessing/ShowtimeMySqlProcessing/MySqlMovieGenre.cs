using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    [Table("movie_genre")]
    public class MySqlMovieGenre
    {
        [Key]
        public int Movie_ID { get; set; }
        public string? Movie_Genre { get; set; }
    }
}
