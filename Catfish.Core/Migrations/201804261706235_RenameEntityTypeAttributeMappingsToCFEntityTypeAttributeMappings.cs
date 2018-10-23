namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEntityTypeAttributeMappingsToCFEntityTypeAttributeMappings : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EntityTypeAttributeMappings", newName: "CFEntityTypeAttributeMappings");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CFEntityTypeAttributeMappings", newName: "EntityTypeAttributeMappings");
        }
    }
}
