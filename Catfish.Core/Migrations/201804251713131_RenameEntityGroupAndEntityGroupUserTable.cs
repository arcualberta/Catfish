namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEntityGroupAndEntityGroupUserTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EntityGroupUsers", "EntityGroupId", "dbo.EntityGroups");
            DropIndex("dbo.EntityGroupUsers", new[] { "EntityGroupId" });
            CreateTable(
                "dbo.CFUserListEntries",
                c => new
                    {
                        CFUserListId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CFUserListId, t.UserId })
                .ForeignKey("dbo.CFUserLists", t => t.CFUserListId, cascadeDelete: true)
                .Index(t => t.CFUserListId);
            
            CreateTable(
                "dbo.CFUserLists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.EntityGroups");
            DropTable("dbo.EntityGroupUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EntityGroupUsers",
                c => new
                    {
                        EntityGroupId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityGroupId, t.UserId });
            
            CreateTable(
                "dbo.EntityGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CFUserListEntries", "CFUserListId", "dbo.CFUserLists");
            DropIndex("dbo.CFUserListEntries", new[] { "CFUserListId" });
            DropTable("dbo.CFUserLists");
            DropTable("dbo.CFUserListEntries");
            CreateIndex("dbo.EntityGroupUsers", "EntityGroupId");
            AddForeignKey("dbo.EntityGroupUsers", "EntityGroupId", "dbo.EntityGroups", "Id", cascadeDelete: true);
        }
    }
}
