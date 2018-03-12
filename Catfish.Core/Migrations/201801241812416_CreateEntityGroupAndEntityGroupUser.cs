namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEntityGroupAndEntityGroupUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntityGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntityGroupUsers",
                c => new
                    {
                        EntityGroupId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityGroupId, t.UserId })
                .ForeignKey("dbo.EntityGroups", t => t.EntityGroupId, cascadeDelete: true)
                .Index(t => t.EntityGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntityGroupUsers", "EntityGroupId", "dbo.EntityGroups");
            DropIndex("dbo.EntityGroupUsers", new[] { "EntityGroupId" });
            DropTable("dbo.EntityGroupUsers");
            DropTable("dbo.EntityGroups");
        }
    }
}
