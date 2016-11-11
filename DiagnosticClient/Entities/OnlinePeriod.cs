using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace DiagnosticClient
{
    [Table("OnlinePeriods", Schema = "Diagnostics")]
    public class OnlinePeriod
    {
        public OnlinePeriod()
        {

        }
        public int Id { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
    }
}