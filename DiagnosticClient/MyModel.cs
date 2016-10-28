namespace DiagnosticClient
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MyModel : DbContext
    {
    
        public MyModel()
            : base("name=MyModel")
        {
        }
        public virtual DbSet<Node> Nodes { get; set; } 
        public virtual DbSet<NodeGroup> NodeGroups { get; set; }
        public virtual DbSet<NodeOnlinePeriod> NodesOnlinePeriods { get; set; }
        public virtual DbSet<NodeLog> NodesLogs { get; set; }
       
    }
}