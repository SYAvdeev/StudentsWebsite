namespace StudentsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Enrollment", "Grade", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Enrollment", "Grade", c => c.Int(nullable: false));
        }
    }
}
