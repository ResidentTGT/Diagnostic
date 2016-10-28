using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace DiagnosticClient
{
    public class NodeLog
    {

        public NodeLog()
        {

        }
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int? Level { get; set; }

        [MaxLength(8000),Required]
        public string Message { get; set; }

        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public Node Node { get; set; }
    }
}
