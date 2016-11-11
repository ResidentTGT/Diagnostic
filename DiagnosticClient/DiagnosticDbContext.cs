namespace DiagnosticClient
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public partial class DiagnosticDbContext : DbContext
    {
        

        public DiagnosticDbContext()
            : base("name=DiagnosticsDbContext")
        {
        }
      
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<OnlinePeriod> NodesOnlinePeriods { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().HasOptional(p => p.Group)
                .WithMany(b => b.Nodes)
                .HasForeignKey(p => p.NodeGroupId);

            modelBuilder.Entity<Log>().HasRequired(p => p.Node)
                .WithMany(b => b.Logs)
                .HasForeignKey(p => p.NodeId);

            modelBuilder.Entity<OnlinePeriod>().HasRequired(p => p.Node)
                .WithMany(b => b.OnlinePeriods)
                .HasForeignKey(p => p.NodeId);
        }       
    }
}
