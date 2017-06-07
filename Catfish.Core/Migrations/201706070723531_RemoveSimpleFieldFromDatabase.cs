namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSimpleFieldFromDatabase : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SimpleFields");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SimpleFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsRequired = c.Boolean(nullable: false),
                        ToolTip = c.String(),
                        MetadataSetId = c.Int(nullable: false),
                        Options = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
