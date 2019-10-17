namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onlineCheckProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Online", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastLogedIn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastLogedIn");
            DropColumn("dbo.AspNetUsers", "Online");
        }
    }
}
