namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsRequiredAndToolTipToMetadataField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetadataFields", "IsRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.MetadataFields", "ToolTip", c => c.String());
            AlterColumn("dbo.MetadataFields", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MetadataFields", "Name", c => c.String());
            DropColumn("dbo.MetadataFields", "ToolTip");
            DropColumn("dbo.MetadataFields", "IsRequired");
        }
    }
}
