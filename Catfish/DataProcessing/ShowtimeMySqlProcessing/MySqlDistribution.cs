using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    [Table("distribution")]
    public class MySqlDistribution
    {
        [Key]
        public int Movie_ID { get; set; }
        public string? Intl_Country { get; set; }
        public string? Intl_Title { get; set; }
        public string? Intl_Rating { get; set; }
        public DateTime? Intl_Release_Date { get; set; }
    }
}
