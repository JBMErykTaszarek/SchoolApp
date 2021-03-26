namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
       
            
            CreateTable(
                "dbo.SchoolTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        description = c.String(),
                        whichClas = c.String(),
                    })
                .PrimaryKey(t => t.Id);
         
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SchoolTasks");
            
        }
    }
}
