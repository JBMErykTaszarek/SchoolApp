namespace SchoolApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class note : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Note",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ForTask = c.String(),
                    WchitNote = c.String(),
                    email = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
        }
    }
}
