namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postedjobon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "PostedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "PostedOn");
        }
    }
}
