namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotifyEmployers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentProfileId = c.Int(nullable: false),
                        ResumeAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotifyInstructors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentProfileId = c.Int(nullable: false),
                        EmployerProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotifyStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployerProfileId = c.Int(nullable: false),
                        RequestedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Notifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Discription = c.String(),
                        IntendedUser = c.Int(nullable: false),
                        UrlPath = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.NotifyStudents");
            DropTable("dbo.NotifyInstructors");
            DropTable("dbo.NotifyEmployers");
        }
    }
}
