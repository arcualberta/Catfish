namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLabelPropertyToEntityTypeAttributeMapping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntityTypeAttributeMappings", "Label", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntityTypeAttributeMappings", "Label");
        }
    }
}
