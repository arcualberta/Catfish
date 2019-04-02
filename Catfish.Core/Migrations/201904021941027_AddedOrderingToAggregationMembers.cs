namespace Catfish.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderingToAggregationMembers : DbMigration
    {
        public override void Up()
        {
            string[] columns = new string[] { "ParentId", "ChildId", "Order" };

            AddColumn("dbo.AggregationHasMembers", "Order", c => c.Int(nullable: false, defaultValue: 0));
            DropPrimaryKey("dbo.AggregationHasMembers");
            AddPrimaryKey("dbo.AggregationHasMembers", columns, "PK_dbo.AggregationHasMembers");
        }
        
        public override void Down()
        {
            string[] columns = new string[] { "ParentId", "ChildId", "Order" };

            DropPrimaryKey("dbo.AggregationHasMembers");
            AddPrimaryKey("dbo.AggregationHasMembers", columns, "PK_dbo.AggregationHasMembers");
            DropColumn("dbo.AggregationHasMembers", "Order");
        }
    }
}
