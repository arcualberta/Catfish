namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEntityTypeToCFEntityType : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EntityTypes", newName: "CFEntityTypes");
            RenameColumn(table: "dbo.EntityTypeAttributeMappings", name: "EntityType_Id", newName: "CFEntityType_Id");
            RenameIndex(table: "dbo.EntityTypeAttributeMappings", name: "IX_EntityType_Id", newName: "IX_CFEntityType_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EntityTypeAttributeMappings", name: "IX_CFEntityType_Id", newName: "IX_EntityType_Id");
            RenameColumn(table: "dbo.EntityTypeAttributeMappings", name: "CFEntityType_Id", newName: "EntityType_Id");
            RenameTable(name: "dbo.CFEntityTypes", newName: "EntityTypes");
        }
    }
}
