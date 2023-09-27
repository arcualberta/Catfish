using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    [Table("country_origin")]
    public class MySqlCountryOrigin : MySqlModel
    {
        [Key]
        public int Movie_ID { get; set; }
        public string? Movie_Title { get; set; }
        public int? IMDb_ID { get; set; }
        public string? IMDb_Title { get; set; }
        public string? Movie_Origin { get; set; }
        public int? Parent_ID { get; set; }
        public string? Year { get; set; }

    }
}
