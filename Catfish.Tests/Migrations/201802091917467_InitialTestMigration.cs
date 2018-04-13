namespace Catfish.Tests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTestMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "XmlModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MappedGuid = c.String(),
                        Content = c.String(),
                        EntityTypeId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("EntityTypes", t => t.EntityTypeId)
                .Index(t => t.EntityTypeId);
            
            CreateTable(
                "EntityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TargetTypes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "EntityTypeAttributeMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MetadataSetId = c.Int(nullable: false),
                        FieldName = c.String(),
                        EntityType_Id = c.Int(),
                        Label = c.String()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("XmlModels", t => t.MetadataSetId, cascadeDelete: true)
                .ForeignKey("EntityTypes", t => t.EntityType_Id)
                .Index(t => t.MetadataSetId)
                .Index(t => t.EntityType_Id);
            
            CreateTable(
                "AggregationHasMembers",
                c => new
                    {
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("XmlModels", t => t.ParentId)
                .ForeignKey("XmlModels", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "AggregationHasRelatedObjects",
                c => new
                    {
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("XmlModels", t => t.ParentId)
                .ForeignKey("XmlModels", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "EntityTypeHasMetadataSets",
                c => new
                    {
                        EntityTypesId = c.Int(nullable: false),
                        MetadataSetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityTypesId, t.MetadataSetId })
                .ForeignKey("EntityTypes", t => t.EntityTypesId, cascadeDelete: true)
                .ForeignKey("XmlModels", t => t.MetadataSetId, cascadeDelete: true)
                .Index(t => t.EntityTypesId)
                .Index(t => t.MetadataSetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("XmlModels", "EntityTypeId", "EntityTypes");
            DropForeignKey("EntityTypeHasMetadataSets", "MetadataSetId", "XmlModels");
            DropForeignKey("EntityTypeHasMetadataSets", "EntityTypesId", "EntityTypes");
            DropForeignKey("EntityTypeAttributeMappings", "EntityType_Id", "EntityTypes");
            DropForeignKey("EntityTypeAttributeMappings", "MetadataSetId", "XmlModels");
            DropForeignKey("AggregationHasRelatedObjects", "ChildId", "XmlModels");
            DropForeignKey("AggregationHasRelatedObjects", "ParentId", "XmlModels");
            DropForeignKey("AggregationHasMembers", "ChildId", "XmlModels");
            DropForeignKey("AggregationHasMembers", "ParentId", "XmlModels");
            DropIndex("XmlModels", new[] { "EntityTypeId" });
            DropIndex("EntityTypeHasMetadataSets", new[] { "MetadataSetId" });
            DropIndex("EntityTypeHasMetadataSets", new[] { "EntityTypesId" });
            DropIndex("EntityTypeAttributeMappings", new[] { "EntityType_Id" });
            DropIndex("EntityTypeAttributeMappings", new[] { "MetadataSetId" });
            DropIndex("AggregationHasRelatedObjects", new[] { "ChildId" });
            DropIndex("AggregationHasRelatedObjects", new[] { "ParentId" });
            DropIndex("AggregationHasMembers", new[] { "ChildId" });
            DropIndex("AggregationHasMembers", new[] { "ParentId" });
            DropTable("EntityTypeHasMetadataSets");
            DropTable("AggregationHasRelatedObjects");
            DropTable("AggregationHasMembers");
            DropTable("EntityTypeAttributeMappings");
            DropTable("EntityTypes");
            DropTable("XmlModels");
        }
    }
}
