namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotifyEmployerAddedFieldsForFunctionalities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyEmployers", "jobResponse", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyEmployers", "JobId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotifyEmployers", "JobId");
            DropColumn("dbo.NotifyEmployers", "jobResponse");
        }
    }
}
