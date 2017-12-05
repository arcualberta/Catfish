namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGuidToXmlModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.XmlModels", "Guid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.XmlModels", "Guid");
        }
    }
}
