namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFieldValueModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FieldValues", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.FieldValues", "FieldId", "dbo.SimpleFields");
            DropIndex("dbo.FieldValues", new[] { "FieldId" });
            DropIndex("dbo.FieldValues", new[] { "EntityId" });
            DropTable("dbo.FieldValues");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        FieldId = c.Int(nullable: false),
                        EntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.FieldValues", "EntityId");
            CreateIndex("dbo.FieldValues", "FieldId");
            AddForeignKey("dbo.FieldValues", "FieldId", "dbo.SimpleFields", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FieldValues", "EntityId", "dbo.Entities", "Id", cascadeDelete: true);
        }
    }
}
