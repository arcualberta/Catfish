using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    [Table("movie_cast")]
    public class MySqlMovieCast
    {
        [Key]
        public int Movie_ID { get; set; }
        public string? Cast_Name { get; set; }
        public string? Cast_Type { get; set; }
    }
}
