namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedXmlContentPropertyToMetadataSet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetadataSets", "Content", c => c.String(storeType: "xml"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetadataSets", "Content");
        }
    }
}
