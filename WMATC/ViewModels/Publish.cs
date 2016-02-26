using System.ComponentModel.DataAnnotations;

namespace WMATC.ViewModels
{
    public class PublishViewModel
    {
        [Key]
        public int EventId { get; set; }
        public string RoundsURL { get; set; }
        public string StandingsURL { get; set; }
        public string TeamBrowser { get; set; }
    }
}