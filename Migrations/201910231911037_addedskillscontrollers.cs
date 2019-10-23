namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedskillscontrollers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentProfiles", "Event_Id", "dbo.Events");
            DropIndex("dbo.StudentProfiles", new[] { "Event_Id" });
            CreateTable(
                "dbo.EventStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Event_Id = c.Int(nullable: false),
                        Student_profileId = c.Int(nullable: false),
                        Event_Id1 = c.Int(),
                        StudentProfile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentProfiles", t => t.StudentProfile_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id1)
                .Index(t => t.Event_Id1)
                .Index(t => t.StudentProfile_Id);
            
            AddColumn("dbo.Events", "NeededVolenteers", c => c.Int(nullable: false));
            DropColumn("dbo.StudentProfiles", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentProfiles", "Event_Id", c => c.Int());
            DropForeignKey("dbo.EventStudents", "Event_Id1", "dbo.Events");
            DropForeignKey("dbo.EventStudents", "StudentProfile_Id", "dbo.StudentProfiles");
            DropIndex("dbo.EventStudents", new[] { "StudentProfile_Id" });
            DropIndex("dbo.EventStudents", new[] { "Event_Id1" });
            DropColumn("dbo.Events", "NeededVolenteers");
            DropTable("dbo.EventStudents");
            CreateIndex("dbo.StudentProfiles", "Event_Id");
            AddForeignKey("dbo.StudentProfiles", "Event_Id", "dbo.Events", "Id");
        }
    }
}
