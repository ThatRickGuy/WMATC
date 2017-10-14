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
        public Event()
        {
            EventDate = DateTime.Today;
            ListLockDate = DateTime.Today;
        }

        [Required]
        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        
        public Guid Owner { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ListLockDate { get; set; }
        public string ImageURL { get; set; }

        public string JSONDump { get; set; }
    }
}

