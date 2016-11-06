using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticClient
{
    [Table("Nodes", Schema = "Diagnostics")]
    public class Node
    {
        public Node()
        {
            NodeLogs = new HashSet<NodeLog>();
            NodeOnlinePeriods = new HashSet<NodeOnlinePeriod>();
        }

        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(128)]
        public string Name { get; set; }


        public NodeGroup NodeGroup { get; set; }
        public int? NodeGroupId { get; set; }

        public virtual ICollection<NodeOnlinePeriod> NodeOnlinePeriods { get; set; }
        public virtual ICollection<NodeLog> NodeLogs { get; set; }
    }
}
