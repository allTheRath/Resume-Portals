namespace Resume_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programwithjoblist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ProgramId", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "ProgramId");
            AddForeignKey("dbo.Jobs", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "ProgramId", "dbo.Programs");
            DropIndex("dbo.Jobs", new[] { "ProgramId" });
            DropColumn("dbo.Jobs", "ProgramId");
        }
    }
}
