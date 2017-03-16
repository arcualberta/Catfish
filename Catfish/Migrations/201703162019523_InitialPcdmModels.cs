namespace Catfish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialPcdmModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aggregations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.Aggregations", t => t.ParentId)
                .ForeignKey("dbo.Aggregations", t => t.ChildId)
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
                .ForeignKey("dbo.Aggregations", t => t.ParentId)
                .ForeignKey("dbo.Aggregations", t => t.ChildId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AggregationHasRelatedObjects", "ChildId", "dbo.Aggregations");
            DropForeignKey("dbo.AggregationHasRelatedObjects", "ParentId", "dbo.Aggregations");
            DropForeignKey("dbo.AggregationHasMembers", "ChildId", "dbo.Aggregations");
            DropForeignKey("dbo.AggregationHasMembers", "ParentId", "dbo.Aggregations");
            DropIndex("dbo.AggregationHasRelatedObjects", new[] { "ChildId" });
            DropIndex("dbo.AggregationHasRelatedObjects", new[] { "ParentId" });
            DropIndex("dbo.AggregationHasMembers", new[] { "ChildId" });
            DropIndex("dbo.AggregationHasMembers", new[] { "ParentId" });
            DropTable("dbo.AggregationHasRelatedObjects");
            DropTable("dbo.AggregationHasMembers");
            DropTable("dbo.Aggregations");
        }
    }
}
