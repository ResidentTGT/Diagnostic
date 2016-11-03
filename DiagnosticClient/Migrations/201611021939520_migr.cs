namespace DiagnosticClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NodeGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        NodeGroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NodeGroups", t => t.NodeGroupId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.NodeGroupId);
            
            CreateTable(
                "dbo.NodeOnlinePeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NodeId = c.Int(nullable: false),
                        TimeStart = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nodes", t => t.NodeId, cascadeDelete: true)
                .Index(t => t.NodeId);
            
            CreateTable(
                "dbo.NodeLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NodeId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Level = c.String(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nodes", t => t.NodeId, cascadeDelete: true)
                .Index(t => t.NodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NodeLogs", "NodeId", "dbo.Nodes");
            DropForeignKey("dbo.NodeOnlinePeriods", "NodeId", "dbo.Nodes");
            DropForeignKey("dbo.Nodes", "NodeGroupId", "dbo.NodeGroups");
            DropIndex("dbo.NodeLogs", new[] { "NodeId" });
            DropIndex("dbo.NodeOnlinePeriods", new[] { "NodeId" });
            DropIndex("dbo.Nodes", new[] { "NodeGroupId" });
            DropIndex("dbo.Nodes", new[] { "Name" });
            DropIndex("dbo.NodeGroups", new[] { "Name" });
            DropTable("dbo.NodeLogs");
            DropTable("dbo.NodeOnlinePeriods");
            DropTable("dbo.Nodes");
            DropTable("dbo.NodeGroups");
        }
    }
}
