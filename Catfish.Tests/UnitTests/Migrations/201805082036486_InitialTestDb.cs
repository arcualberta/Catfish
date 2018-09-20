namespace Catfish.Tests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTestDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CFXmlModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MappedGuid = c.String(),
                        Content = c.String(),
                        EntityTypeId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("CFEntityTypes", t => t.EntityTypeId)
                .Index(t => t.EntityTypeId);
            
            CreateTable(
                "CFEntityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TargetTypes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "CFEntityTypeAttributeMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MetadataSetId = c.Int(nullable: false),
                        FieldName = c.String(),
                        Label = c.String(),
                        CFEntityType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("CFXmlModels", t => t.MetadataSetId, cascadeDelete: true)
                .ForeignKey("CFEntityTypes", t => t.CFEntityType_Id)
                .Index(t => t.MetadataSetId)
                .Index(t => t.CFEntityType_Id);
            
            CreateTable(
                "CFUserListEntries",
                c => new
                    {
                        CFUserListId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CFUserListId, t.UserId })
                .ForeignKey("CFUserLists", t => t.CFUserListId, cascadeDelete: true)
                .Index(t => t.CFUserListId);
            
            CreateTable(
                "CFUserLists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "AggregationHasMembers",
                c => new
                    {
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("CFXmlModels", t => t.ParentId)
                .ForeignKey("CFXmlModels", t => t.ChildId)
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
                .ForeignKey("CFXmlModels", t => t.ParentId)
                .ForeignKey("CFXmlModels", t => t.ChildId)
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
                .ForeignKey("CFEntityTypes", t => t.EntityTypesId, cascadeDelete: true)
                .ForeignKey("CFXmlModels", t => t.MetadataSetId, cascadeDelete: true)
                .Index(t => t.EntityTypesId)
                .Index(t => t.MetadataSetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("CFUserListEntries", "CFUserListId", "CFUserLists");
            DropForeignKey("CFXmlModels", "EntityTypeId", "CFEntityTypes");
            DropForeignKey("EntityTypeHasMetadataSets", "MetadataSetId", "CFXmlModels");
            DropForeignKey("EntityTypeHasMetadataSets", "EntityTypesId", "CFEntityTypes");
            DropForeignKey("CFEntityTypeAttributeMappings", "CFEntityType_Id", "CFEntityTypes");
            DropForeignKey("CFEntityTypeAttributeMappings", "MetadataSetId", "CFXmlModels");
            DropForeignKey("AggregationHasRelatedObjects", "ChildId", "CFXmlModels");
            DropForeignKey("AggregationHasRelatedObjects", "ParentId", "CFXmlModels");
            DropForeignKey("AggregationHasMembers", "ChildId", "CFXmlModels");
            DropForeignKey("AggregationHasMembers", "ParentId", "CFXmlModels");
            DropIndex("CFUserListEntries", new[] { "CFUserListId" });
            DropIndex("CFXmlModels", new[] { "EntityTypeId" });
            DropIndex("EntityTypeHasMetadataSets", new[] { "MetadataSetId" });
            DropIndex("EntityTypeHasMetadataSets", new[] { "EntityTypesId" });
            DropIndex("CFEntityTypeAttributeMappings", new[] { "CFEntityType_Id" });
            DropIndex("CFEntityTypeAttributeMappings", new[] { "MetadataSetId" });
            DropIndex("AggregationHasRelatedObjects", new[] { "ChildId" });
            DropIndex("AggregationHasRelatedObjects", new[] { "ParentId" });
            DropIndex("AggregationHasMembers", new[] { "ChildId" });
            DropIndex("AggregationHasMembers", new[] { "ParentId" });
            DropTable("EntityTypeHasMetadataSets");
            DropTable("AggregationHasRelatedObjects");
            DropTable("AggregationHasMembers");
            DropTable("CFUserLists");
            DropTable("CFUserListEntries");
            DropTable("CFEntityTypeAttributeMappings");
            DropTable("CFEntityTypes");
            DropTable("CFXmlModels");
        }
    }
}
