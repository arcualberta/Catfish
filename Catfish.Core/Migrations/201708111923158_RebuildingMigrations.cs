namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RebuildingMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.XmlModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityTypeId = c.Int(),
                        Content = c.String(storeType: "xml"),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityTypes", t => t.EntityTypeId)
                .Index(t => t.EntityTypeId);
            
            CreateTable(
                "dbo.EntityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AggregationHasMembers",
                c => new
                    {
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("dbo.XmlModels", t => t.ParentId)
                .ForeignKey("dbo.XmlModels", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "dbo.EntityTypeHasMetadataSets",
                c => new
                    {
                        EntityTypesId = c.Int(nullable: false),
                        MetadataSetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityTypesId, t.MetadataSetId })
                .ForeignKey("dbo.EntityTypes", t => t.EntityTypesId, cascadeDelete: true)
                .ForeignKey("dbo.XmlModels", t => t.MetadataSetId, cascadeDelete: true)
                .Index(t => t.EntityTypesId)
                .Index(t => t.MetadataSetId);
            
            CreateTable(
                "dbo.AggregationHasRelatedObjects",
                c => new
                    {
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("dbo.XmlModels", t => t.ParentId)
                .ForeignKey("dbo.XmlModels", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.XmlModels", "EntityTypeId", "dbo.EntityTypes");
            DropForeignKey("dbo.AggregationHasRelatedObjects", "ChildId", "dbo.XmlModels");
            DropForeignKey("dbo.AggregationHasRelatedObjects", "ParentId", "dbo.XmlModels");
            DropForeignKey("dbo.EntityTypeHasMetadataSets", "MetadataSetId", "dbo.XmlModels");
            DropForeignKey("dbo.EntityTypeHasMetadataSets", "EntityTypesId", "dbo.EntityTypes");
            DropForeignKey("dbo.AggregationHasMembers", "ChildId", "dbo.XmlModels");
            DropForeignKey("dbo.AggregationHasMembers", "ParentId", "dbo.XmlModels");
            DropIndex("dbo.AggregationHasRelatedObjects", new[] { "ChildId" });
            DropIndex("dbo.AggregationHasRelatedObjects", new[] { "ParentId" });
            DropIndex("dbo.EntityTypeHasMetadataSets", new[] { "MetadataSetId" });
            DropIndex("dbo.EntityTypeHasMetadataSets", new[] { "EntityTypesId" });
            DropIndex("dbo.AggregationHasMembers", new[] { "ChildId" });
            DropIndex("dbo.AggregationHasMembers", new[] { "ParentId" });
            DropIndex("dbo.XmlModels", new[] { "EntityTypeId" });
            DropTable("dbo.AggregationHasRelatedObjects");
            DropTable("dbo.EntityTypeHasMetadataSets");
            DropTable("dbo.AggregationHasMembers");
            DropTable("dbo.EntityTypes");
            DropTable("dbo.XmlModels");
        }
    }
}
