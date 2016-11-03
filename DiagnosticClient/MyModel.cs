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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().HasOptional(p => p.NodeGroup)
                .WithMany(b => b.Nodes)
                .HasForeignKey(p => p.NodeGroupId);

            modelBuilder.Entity<NodeLog>().HasRequired(p => p.Node)
                .WithMany(b => b.NodesLogs)
                .HasForeignKey(p => p.NodeId);

            modelBuilder.Entity<NodeOnlinePeriod>().HasRequired(p => p.Node)
                .WithMany(b => b.NodeOnlinePeriods)
                .HasForeignKey(p => p.NodeId);
        }

        static public void ReadNodes()
        {
            MyModel context = new MyModel();
            var list = context.Nodes.ToList();
            Console.WriteLine("Nodes");
            foreach (var item in list)
                Console.WriteLine("{0} | Name:{1} | GroupId:{2}", item.Id, item.Name, item.NodeGroupId);
        }

        static public void ReadNodeGroups()
        {
            MyModel context = new MyModel();
            var list = context.NodeGroups.ToList();
            Console.WriteLine("NodeGroups");
            foreach (var item in list)
                Console.WriteLine("{0} | Name:{1}", item.Id, item.Name);
        }

        static public void ReadNodeOnlinePeriods()
        {
            MyModel context = new MyModel();
            var list = context.NodesOnlinePeriods.ToList();
            Console.WriteLine("NodeOnlinePeriods");
            foreach (var item in list)
                Console.WriteLine("{0} | NodeId: {1} | Start:{2} | End:{3}", item.Id, item.NodeId, item.TimeStart, item.TimeEnd);
        }

        static public int GetGroupId(string group_name)
        {
            MyModel context = new MyModel();
            var list = context.NodeGroups.ToList();
            foreach (var Nodegroup in list)
            {
                if (Nodegroup.Name == group_name)
                {
                    return Nodegroup.Id;
                }
            }
            return 0;
        }

        static public int GetNodeId(string node_name)
        {
            MyModel context = new MyModel();
            var list = context.Nodes.ToList();
            foreach (var node in list)
            {
                if (node.Name == node_name)
                {
                    return node.Id;
                }
            }
            return 0;
        }

    }
}