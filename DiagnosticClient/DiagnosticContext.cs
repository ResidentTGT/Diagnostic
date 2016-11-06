namespace DiagnosticClient
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    internal class DiagnosticContext : DbContext
    {

        public DiagnosticContext()
            : base("name=DiagnosticContext")
        {
        }

        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<NodeGroup> NodeGroups { get; set; }
        public virtual DbSet<NodeOnlinePeriod> NodesOnlinePeriods { get; set; }
        public virtual DbSet<NodeLog> NodeLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().HasOptional(p => p.NodeGroup)
                .WithMany(b => b.Nodes)
                .HasForeignKey(p => p.NodeGroupId);

            modelBuilder.Entity<NodeLog>().HasRequired(p => p.Node)
                .WithMany(b => b.NodeLogs)
                .HasForeignKey(p => p.NodeId);

            modelBuilder.Entity<NodeOnlinePeriod>().HasRequired(p => p.Node)
                .WithMany(b => b.NodeOnlinePeriods)
                .HasForeignKey(p => p.NodeId);
        }

        static public void ReadNodes()
        {
            DiagnosticContext context = new DiagnosticContext();
            var list = context.Nodes.ToList();
            Console.WriteLine("Nodes");
            foreach (var item in list)
                Console.WriteLine("{0} | Name:{1} | GroupId:{2}", item.Id, item.Name, item.NodeGroupId);
        }

        static public void ReadNodeGroups()
        {
            DiagnosticContext context = new DiagnosticContext();
            var list = context.NodeGroups.ToList();
            Console.WriteLine("NodeGroups");
            foreach (var item in list)
                Console.WriteLine("{0} | Name:{1}", item.Id, item.Name);
        }

        static public void ReadNodeOnlinePeriods()
        {
            DiagnosticContext context = new DiagnosticContext();
            var list = context.NodesOnlinePeriods.ToList();
            Console.WriteLine("NodeOnlinePeriods");
            foreach (var item in list)
                Console.WriteLine("{0} | NodeId: {1} | Start:{2} | End:{3}", item.Id, item.NodeId, item.StartTime, item.EndTime);
        }


        static public int GetGroupId(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException();

            NodeGroup result;
            using (var ctx = new DiagnosticContext())
                result = ctx.NodeGroups.Where(g => g.Name == groupName).FirstOrDefault();

            if (result == null)
                throw new NullReferenceException();
            else
                return result.Id;
        }

        static public int GetNodeId(string nodeName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new InvalidOperationException("Node name must be not empty");

            Node result;
            using (var ctx = new DiagnosticContext())
                result = ctx.Nodes.Where(g => g.Name == nodeName).FirstOrDefault();

            if (result == null)
                throw new NullReferenceException();
            else
                return result.Id;
        }
    }
}