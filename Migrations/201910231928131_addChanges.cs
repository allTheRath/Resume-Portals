namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentProfiles", "Event_Id", "dbo.Events");
            DropIndex("dbo.StudentProfiles", new[] { "Event_Id" });
            RenameColumn(table: "dbo.EventStudents", name: "Event_Id", newName: "EventId");
            CreateTable(
                "dbo.EventStudents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        studentprofileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentProfiles", t => t.studentprofileId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.studentprofileId);
            
            AddColumn("dbo.Events", "NeededVolenteers", c => c.Int(nullable: false));
            DropColumn("dbo.StudentProfiles", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentProfiles", "Event_Id", c => c.Int());
            DropForeignKey("dbo.EventStudents", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventStudents", "studentprofileId", "dbo.StudentProfiles");
            DropIndex("dbo.EventStudents", new[] { "studentprofileId" });
            DropIndex("dbo.EventStudents", new[] { "EventId" });
            DropColumn("dbo.Events", "NeededVolenteers");
            DropTable("dbo.EventStudents");
            RenameColumn(table: "dbo.EventStudents", name: "EventId", newName: "Event_Id");
            CreateIndex("dbo.StudentProfiles", "Event_Id");
            AddForeignKey("dbo.StudentProfiles", "Event_Id", "dbo.Events", "Id");
        }
    }
}
