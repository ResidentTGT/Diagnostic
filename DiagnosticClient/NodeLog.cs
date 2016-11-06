using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace DiagnosticClient
{
    [Table("Logs", Schema = "Diagnostics")]
    public class NodeLog
    {
        public NodeLog()
        {
        }
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int NodeId { get; set; }
        public Node Node { get; set; }

        [Required]
        public string Level { get; set; }

        [MaxLength(8000), Required]
        public string Message { get; set; }

    }
}
