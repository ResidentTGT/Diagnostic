namespace DiagnosticClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NodeLogs", "Time", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NodeLogs", "Time", c => c.DateTime(nullable: false));
        }
    }
}
