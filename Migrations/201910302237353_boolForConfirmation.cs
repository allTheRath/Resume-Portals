namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boolForConfirmation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyEmployers", "confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyInstructors", "confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyStudents", "confirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotifyStudents", "confirmed");
            DropColumn("dbo.NotifyInstructors", "confirmed");
            DropColumn("dbo.NotifyEmployers", "confirmed");
        }
    }
}
