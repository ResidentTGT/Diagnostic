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
        public int NodeId { get; set; }
        public Node Node { get; set; }

        public int Id { get; set; }
        public DateTime Time { get; set; }

        [Required]
        public string Level { get; set; }

        [MaxLength(8000),Required]
        public string Message { get; set; }

        void SaveLog(int NodeId,string Message, DateTime Time)
        {

        }
 
    }
}
