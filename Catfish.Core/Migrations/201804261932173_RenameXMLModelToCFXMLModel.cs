namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameXMLModelToCFXMLModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.XmlModels", newName: "CFXmlModels");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CFXmlModels", newName: "XmlModels");
        }
    }
}
