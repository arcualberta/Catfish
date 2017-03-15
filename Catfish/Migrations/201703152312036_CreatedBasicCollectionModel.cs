namespace Catfish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBasicCollectionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Collections");
        }
    }
}
