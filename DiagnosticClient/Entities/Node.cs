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
            Logs = new HashSet<Log>();
            OnlinePeriods = new HashSet<OnlinePeriod>();
        }

        public int Id { get; set; }

        [Required]
        [Index("IX_Index", 1, IsUnique = true)]
        [MaxLength(128)]
        public string Name { get; set; }


        public Group Group { get; set; }
        [Index("IX_Index", 2, IsUnique = true)]
        public int? NodeGroupId { get; set; }

        public virtual ICollection<OnlinePeriod> OnlinePeriods { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
