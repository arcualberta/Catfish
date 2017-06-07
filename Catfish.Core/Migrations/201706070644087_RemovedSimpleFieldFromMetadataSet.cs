namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSimpleFieldFromMetadataSet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SimpleFields", "MetadataSetId", "dbo.MetadataSets");
            DropIndex("dbo.SimpleFields", new[] { "MetadataSetId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.SimpleFields", "MetadataSetId");
            AddForeignKey("dbo.SimpleFields", "MetadataSetId", "dbo.MetadataSets", "Id", cascadeDelete: true);
        }
    }
}
