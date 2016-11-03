using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticClient
{
    public class Node
    {
        public Node()
        {    
        }
        
        public int Id { get; set; }
        
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(128)]
        public string Name { get; set; }

        public NodeGroup NodeGroup { get; set; }
        public int? NodeGroupId { get; set; }

        public virtual ICollection<NodeOnlinePeriod> NodeOnlinePeriods { get; set; }
        public virtual ICollection<NodeLog> NodesLogs { get; set; }
    }
}
