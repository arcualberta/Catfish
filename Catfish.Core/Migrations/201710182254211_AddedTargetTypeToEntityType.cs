namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTargetTypeToEntityType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntityTypes", "TargetType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntityTypes", "TargetType");
        }
    }
}
