namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMetadataModels : DbMigration
    {
        public override void Up()
        {
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
                "dbo.MetadataSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MetadataFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MetadataSetId = c.Int(nullable: false),
                        Options = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MetadataSets", t => t.MetadataSetId, cascadeDelete: true)
                .Index(t => t.MetadataSetId);
            
            CreateTable(
                "dbo.FieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        FieldId = c.Int(nullable: false),
                        EntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entities", t => t.EntityId, cascadeDelete: true)
                .ForeignKey("dbo.MetadataFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.EntityId);
            
            CreateTable(
                "dbo.EntityTypeHasMetadataSets",
                c => new
                    {
                        MetadataSetId = c.Int(nullable: false),
                        EntityTypesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MetadataSetId, t.EntityTypesId })
                .ForeignKey("dbo.EntityTypes", t => t.MetadataSetId, cascadeDelete: true)
                .ForeignKey("dbo.MetadataSets", t => t.EntityTypesId, cascadeDelete: true)
                .Index(t => t.MetadataSetId)
                .Index(t => t.EntityTypesId);
            
            AddColumn("dbo.Entities", "EntityTypeId", c => c.Int());
            CreateIndex("dbo.Entities", "EntityTypeId");
            AddForeignKey("dbo.Entities", "EntityTypeId", "dbo.EntityTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FieldValues", "FieldId", "dbo.MetadataFields");
            DropForeignKey("dbo.FieldValues", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Entities", "EntityTypeId", "dbo.EntityTypes");
            DropForeignKey("dbo.EntityTypeHasMetadataSets", "EntityTypesId", "dbo.MetadataSets");
            DropForeignKey("dbo.EntityTypeHasMetadataSets", "MetadataSetId", "dbo.EntityTypes");
            DropForeignKey("dbo.MetadataFields", "MetadataSetId", "dbo.MetadataSets");
            DropIndex("dbo.EntityTypeHasMetadataSets", new[] { "EntityTypesId" });
            DropIndex("dbo.EntityTypeHasMetadataSets", new[] { "MetadataSetId" });
            DropIndex("dbo.FieldValues", new[] { "EntityId" });
            DropIndex("dbo.FieldValues", new[] { "FieldId" });
            DropIndex("dbo.MetadataFields", new[] { "MetadataSetId" });
            DropIndex("dbo.Entities", new[] { "EntityTypeId" });
            DropColumn("dbo.Entities", "EntityTypeId");
            DropTable("dbo.EntityTypeHasMetadataSets");
            DropTable("dbo.FieldValues");
            DropTable("dbo.MetadataFields");
            DropTable("dbo.MetadataSets");
            DropTable("dbo.EntityTypes");
        }
    }
}
