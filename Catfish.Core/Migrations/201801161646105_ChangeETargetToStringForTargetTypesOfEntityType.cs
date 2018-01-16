namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeETargetToStringForTargetTypesOfEntityType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntityTypes", "TargetTypes", c => c.String());
            DropColumn("dbo.EntityTypes", "TargetType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EntityTypes", "TargetType", c => c.Int(nullable: false));
            DropColumn("dbo.EntityTypes", "TargetTypes");
        }
    }
}
