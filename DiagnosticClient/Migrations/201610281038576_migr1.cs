namespace DiagnosticClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NodeOnlinePeriods", "NodeId", "dbo.Nodes");
            DropIndex("dbo.NodeOnlinePeriods", new[] { "NodeId" });
            AlterColumn("dbo.Nodes", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.NodeOnlinePeriods", "NodeId", c => c.Int(nullable: false));
            AlterColumn("dbo.NodeOnlinePeriods", "TimeStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.NodeOnlinePeriods", "TimeEnd", c => c.DateTime(nullable: false));
            CreateIndex("dbo.NodeOnlinePeriods", "NodeId");
            AddForeignKey("dbo.NodeOnlinePeriods", "NodeId", "dbo.Nodes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NodeOnlinePeriods", "NodeId", "dbo.Nodes");
            DropIndex("dbo.NodeOnlinePeriods", new[] { "NodeId" });
            AlterColumn("dbo.NodeOnlinePeriods", "TimeEnd", c => c.DateTime());
            AlterColumn("dbo.NodeOnlinePeriods", "TimeStart", c => c.DateTime());
            AlterColumn("dbo.NodeOnlinePeriods", "NodeId", c => c.Int());
            AlterColumn("dbo.Nodes", "Name", c => c.String());
            CreateIndex("dbo.NodeOnlinePeriods", "NodeId");
            AddForeignKey("dbo.NodeOnlinePeriods", "NodeId", "dbo.Nodes", "Id");
        }
    }
}
