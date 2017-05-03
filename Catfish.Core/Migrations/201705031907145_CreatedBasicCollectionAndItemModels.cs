namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBasicCollectionAndItemModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(),
                        Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
                .ForeignKey("dbo.Entities", t => t.ParentId)
                .ForeignKey("dbo.Entities", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "dbo.AggregationHasRelatedObjects",
                c => new
                    {
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentId, t.ChildId })
                .ForeignKey("dbo.Entities", t => t.ParentId)
                .ForeignKey("dbo.Entities", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AggregationHasRelatedObjects", "ChildId", "dbo.Entities");
            DropForeignKey("dbo.AggregationHasRelatedObjects", "ParentId", "dbo.Entities");
            DropForeignKey("dbo.AggregationHasMembers", "ChildId", "dbo.Entities");
            DropForeignKey("dbo.AggregationHasMembers", "ParentId", "dbo.Entities");
            DropIndex("dbo.AggregationHasRelatedObjects", new[] { "ChildId" });
            DropIndex("dbo.AggregationHasRelatedObjects", new[] { "ParentId" });
            DropIndex("dbo.AggregationHasMembers", new[] { "ChildId" });
            DropIndex("dbo.AggregationHasMembers", new[] { "ParentId" });
            DropTable("dbo.AggregationHasRelatedObjects");
            DropTable("dbo.AggregationHasMembers");
            DropTable("dbo.Entities");
        }
    }
}
