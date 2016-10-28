namespace DiagnosticClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NodeLogs", "Time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.NodeLogs", "Level", c => c.Int());
            AlterColumn("dbo.NodeLogs", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NodeLogs", "Message", c => c.String());
            AlterColumn("dbo.NodeLogs", "Level", c => c.Int(nullable: false));
            AlterColumn("dbo.NodeLogs", "Time", c => c.DateTime());
        }
    }
}
