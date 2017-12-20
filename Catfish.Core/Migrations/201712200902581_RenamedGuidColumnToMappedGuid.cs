namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedGuidColumnToMappedGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.XmlModels", "MappedGuid", c => c.String());
            DropColumn("dbo.XmlModels", "Guid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.XmlModels", "Guid", c => c.String());
            DropColumn("dbo.XmlModels", "MappedGuid");
        }
    }
}
