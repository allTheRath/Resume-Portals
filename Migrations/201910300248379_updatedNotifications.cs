namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyEmployers", "EmployerId", c => c.String());
            AddColumn("dbo.NotifyInstructors", "InstructorId", c => c.String());
            AddColumn("dbo.NotifyStudents", "studentId", c => c.String());
            AlterColumn("dbo.NotifyEmployers", "StudentProfileId", c => c.String());
            AlterColumn("dbo.NotifyInstructors", "StudentProfileId", c => c.String());
            AlterColumn("dbo.NotifyInstructors", "EmployerProfileId", c => c.String());
            AlterColumn("dbo.NotifyStudents", "EmployerProfileId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NotifyStudents", "EmployerProfileId", c => c.Int(nullable: false));
            AlterColumn("dbo.NotifyInstructors", "EmployerProfileId", c => c.Int(nullable: false));
            AlterColumn("dbo.NotifyInstructors", "StudentProfileId", c => c.Int(nullable: false));
            AlterColumn("dbo.NotifyEmployers", "StudentProfileId", c => c.Int(nullable: false));
            DropColumn("dbo.NotifyStudents", "studentId");
            DropColumn("dbo.NotifyInstructors", "InstructorId");
            DropColumn("dbo.NotifyEmployers", "EmployerId");
        }
    }
}
