namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentProfiles", "MyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentProfiles", "MyName");
        }
    }
}
