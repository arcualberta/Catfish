namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Catfish.Core.Helpers;
    
    public partial class Added_XML_Indexing : DbMigration
    {
        public override void Up()
        {
            this.CreateXmlIndex("dbo.CFXmlModels", "Content", "XmlModel_Primary_Index");
            this.CreateXmlIndex("dbo.CFXmlModels", "Content", "XmlModel_Index_PATH", false, CreateXmlIndexOperation.eIndexType.PATH, "XmlModel_Primary_Index");
            this.CreateXmlIndex("dbo.CFXmlModels", "Content", "XmlModel_Index_VALUE", false, CreateXmlIndexOperation.eIndexType.VALUE, "XmlModel_Primary_Index");
            this.CreateXmlIndex("dbo.CFXmlModels", "Content", "XmlModel_Index_PROPERTY", false, CreateXmlIndexOperation.eIndexType.PROPERTY, "XmlModel_Primary_Index");
        }
        
        public override void Down()
        {
            this.RemoveXmlIndex("dbo.CFXmlModels", "XmlModel_Index_PROPERTY");
            this.RemoveXmlIndex("dbo.CFXmlModels", "XmlModel_Index_VALUE");
            this.RemoveXmlIndex("dbo.CFXmlModels", "XmlModel_Index_PATH");
            this.RemoveXmlIndex("dbo.CFXmlModels", "XmlModel_Primary_Index");
        }
    }
}
