namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connectnetuserwithprofile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotifyAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        UserEmail = c.String(),
                        RoleName = c.String(),
                        RequestedOn = c.DateTime(nullable: false),
                        Resolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NotifyAdmins");
        }
    }
}
