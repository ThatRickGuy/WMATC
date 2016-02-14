using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WMATC.Models
{
    public class Event
    {
        [Required]
        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string ImageURL { get; set; }
    }
}

