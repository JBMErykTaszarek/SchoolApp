namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Notes");
        }
    }
}
