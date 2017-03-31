namespace StudentsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Professor", "UserID", c => c.String(maxLength: 128));
            AddColumn("dbo.Student", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Professor", "UserID");
            CreateIndex("dbo.Student", "UserID");
            AddForeignKey("dbo.Professor", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Student", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Student", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Professor", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Student", new[] { "UserID" });
            DropIndex("dbo.Professor", new[] { "UserID" });
            DropColumn("dbo.Student", "UserID");
            DropColumn("dbo.Professor", "UserID");
        }
    }
}
