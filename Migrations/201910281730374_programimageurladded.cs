namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programimageurladded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "ImageUrl");
        }
    }
}
