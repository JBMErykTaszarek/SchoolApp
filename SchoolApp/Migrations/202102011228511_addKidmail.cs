namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKidmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "kidMail", c => c.String());
            DropColumn("dbo.AspNetUsers", "KidId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "KidId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "kidMail");
        }
    }
}
