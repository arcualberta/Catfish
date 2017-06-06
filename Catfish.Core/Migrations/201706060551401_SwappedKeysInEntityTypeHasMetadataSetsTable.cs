namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwappedKeysInEntityTypeHasMetadataSetsTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.EntityTypeHasMetadataSets", name: "MetadataSetId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.EntityTypeHasMetadataSets", name: "EntityTypesId", newName: "MetadataSetId");
            RenameColumn(table: "dbo.EntityTypeHasMetadataSets", name: "__mig_tmp__0", newName: "EntityTypesId");
            RenameIndex(table: "dbo.EntityTypeHasMetadataSets", name: "IX_MetadataSetId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.EntityTypeHasMetadataSets", name: "IX_EntityTypesId", newName: "IX_MetadataSetId");
            RenameIndex(table: "dbo.EntityTypeHasMetadataSets", name: "__mig_tmp__0", newName: "IX_EntityTypesId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EntityTypeHasMetadataSets", name: "IX_EntityTypesId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.EntityTypeHasMetadataSets", name: "IX_MetadataSetId", newName: "IX_EntityTypesId");
            RenameIndex(table: "dbo.EntityTypeHasMetadataSets", name: "__mig_tmp__0", newName: "IX_MetadataSetId");
            RenameColumn(table: "dbo.EntityTypeHasMetadataSets", name: "EntityTypesId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.EntityTypeHasMetadataSets", name: "MetadataSetId", newName: "EntityTypesId");
            RenameColumn(table: "dbo.EntityTypeHasMetadataSets", name: "__mig_tmp__0", newName: "MetadataSetId");
        }
    }
}
