using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticClient
{
    public class Node
    {
        public Node()
        {
            NodesLogs = new List<NodeLog>();
            NodeOnlinePeriods = new List<NodeOnlinePeriod>();
        }
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public NodeGroup NodeGroup { get; set; }

        public ICollection<NodeOnlinePeriod> NodeOnlinePeriods { get; set; }
        public ICollection<NodeLog> NodesLogs { get; set; }
    }
}
