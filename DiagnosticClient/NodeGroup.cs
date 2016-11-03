using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DiagnosticClient
{
    public class NodeGroup
    {
        public NodeGroup()
        {
        }
        public int Id { get; set; }
        
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(128)]
        public string Name { get; set; }
        
        public virtual ICollection<Node> Nodes { get; set; }
    }
}