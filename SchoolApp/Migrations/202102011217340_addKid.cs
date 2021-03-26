namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "KidId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "KidId");
        }
    }
}
