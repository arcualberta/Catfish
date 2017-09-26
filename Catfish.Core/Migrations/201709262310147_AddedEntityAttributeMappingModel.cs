namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEntityAttributeMappingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntityTypeAttributeMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MetadataSetId = c.Int(nullable: false),
                        FieldName = c.String(),
                        EntityType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.XmlModels", t => t.MetadataSetId, cascadeDelete: true)
                .ForeignKey("dbo.EntityTypes", t => t.EntityType_Id)
                .Index(t => t.MetadataSetId)
                .Index(t => t.EntityType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntityTypeAttributeMappings", "EntityType_Id", "dbo.EntityTypes");
            DropForeignKey("dbo.EntityTypeAttributeMappings", "MetadataSetId", "dbo.XmlModels");
            DropIndex("dbo.EntityTypeAttributeMappings", new[] { "EntityType_Id" });
            DropIndex("dbo.EntityTypeAttributeMappings", new[] { "MetadataSetId" });
            DropTable("dbo.EntityTypeAttributeMappings");
        }
    }
}
