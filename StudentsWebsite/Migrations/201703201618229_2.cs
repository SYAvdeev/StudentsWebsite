namespace StudentsWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ProfessorIndexViewModel");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProfessorIndexViewModel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        NumOfStudents = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
