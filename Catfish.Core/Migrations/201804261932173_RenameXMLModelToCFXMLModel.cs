namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameXMLModelToCFXMLModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.XmlModels", newName: "CFXmlModels");

            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.MetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.CFMetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.Collection, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.CFCollection, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.Item, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.CFItem, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.DataFile, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.CFDataFile, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Discriminator = 'CF' + Discriminator WHERE Discriminator <> 'Form'");
        }

        public override void Down()
        {
            this.Sql("UPDATE[dbo].[CFXmlModels] SET Discriminator = substring(Discriminator, 2) WHERE Discriminator<> 'Form'");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.CFMetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.MetadataSet, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.CFCollection, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.Collection, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.CFItem, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.Item, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");
            this.Sql("UPDATE [dbo].[CFXmlModels] SET Content = REPLACE(CAST(CONTENT as varchar(max)), 'Catfish.Core.Models.CFDataFile, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', 'Catfish.Core.Models.DataFile, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')");

            RenameTable(name: "dbo.CFXmlModels", newName: "XmlModels");     
        }
    }
}
