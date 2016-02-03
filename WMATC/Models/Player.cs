using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WMATC.Models
{
    public class Player
    {
        [Required]
        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }
        [Url]
        public string ImgURL { get; set; }
        public string List1 { get; set; }
        public string List2 { get; set; }
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}