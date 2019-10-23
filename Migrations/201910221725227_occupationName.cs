namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class occupationName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentProfiles", "OccupationName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentProfiles", "OccupationName");
        }
    }
}
