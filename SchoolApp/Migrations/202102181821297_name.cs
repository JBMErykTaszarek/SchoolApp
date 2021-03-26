namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "messageContent", c => c.String());
            DropColumn("dbo.Messages", "message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "message", c => c.String());
            DropColumn("dbo.Messages", "messageContent");
        }
    }
}
