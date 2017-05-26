namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedMetadataFieldoSimpleField : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MetadataFields", newName: "SimpleFields");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SimpleFields", newName: "MetadataFields");
        }
    }
}
