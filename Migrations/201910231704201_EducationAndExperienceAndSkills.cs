namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EducationAndExperienceAndSkills : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        InstituteName = c.String(),
                        Discription = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        StudentProfile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.StudentProfiles", t => t.StudentProfile_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.StudentProfile_Id);
            
            CreateTable(
                "dbo.Experiances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        InstituteName = c.String(),
                        Discription = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        StudentProfile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.StudentProfiles", t => t.StudentProfile_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.StudentProfile_Id);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        SkillName = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        StudentProfile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.StudentProfiles", t => t.StudentProfile_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.StudentProfile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skills", "StudentProfile_Id", "dbo.StudentProfiles");
            DropForeignKey("dbo.Skills", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Experiances", "StudentProfile_Id", "dbo.StudentProfiles");
            DropForeignKey("dbo.Experiances", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Educations", "StudentProfile_Id", "dbo.StudentProfiles");
            DropForeignKey("dbo.Educations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Skills", new[] { "StudentProfile_Id" });
            DropIndex("dbo.Skills", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Experiances", new[] { "StudentProfile_Id" });
            DropIndex("dbo.Experiances", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Educations", new[] { "StudentProfile_Id" });
            DropIndex("dbo.Educations", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Skills");
            DropTable("dbo.Experiances");
            DropTable("dbo.Educations");
        }
    }
}
