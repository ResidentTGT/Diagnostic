namespace DiagnosticClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFirst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Diagnostics.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "Diagnostics.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        NodeGroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Diagnostics.Groups", t => t.NodeGroupId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.NodeGroupId);
            
            CreateTable(
                "Diagnostics.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        NodeId = c.Int(nullable: false),
                        Level = c.String(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Diagnostics.Nodes", t => t.NodeId, cascadeDelete: true)
                .Index(t => t.NodeId);
            
            CreateTable(
                "Diagnostics.OnlinePeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NodeId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Diagnostics.Nodes", t => t.NodeId, cascadeDelete: true)
                .Index(t => t.NodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Diagnostics.OnlinePeriods", "NodeId", "Diagnostics.Nodes");
            DropForeignKey("Diagnostics.Logs", "NodeId", "Diagnostics.Nodes");
            DropForeignKey("Diagnostics.Nodes", "NodeGroupId", "Diagnostics.Groups");
            DropIndex("Diagnostics.OnlinePeriods", new[] { "NodeId" });
            DropIndex("Diagnostics.Logs", new[] { "NodeId" });
            DropIndex("Diagnostics.Nodes", new[] { "NodeGroupId" });
            DropIndex("Diagnostics.Nodes", new[] { "Name" });
            DropIndex("Diagnostics.Groups", new[] { "Name" });
            DropTable("Diagnostics.OnlinePeriods");
            DropTable("Diagnostics.Logs");
            DropTable("Diagnostics.Nodes");
            DropTable("Diagnostics.Groups");
        }
    }
}
