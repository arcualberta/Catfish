namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedNameAndDescriptionFromMetadataSetModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MetadataSets", "Name");
            DropColumn("dbo.MetadataSets", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MetadataSets", "Description", c => c.String());
            AddColumn("dbo.MetadataSets", "Name", c => c.String());
        }
    }
}
