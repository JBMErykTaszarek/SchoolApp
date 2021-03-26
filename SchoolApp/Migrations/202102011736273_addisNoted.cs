namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addisNoted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SchoolTasks", "IsNoted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SchoolTasks", "IsNoted");
        }
    }
}
