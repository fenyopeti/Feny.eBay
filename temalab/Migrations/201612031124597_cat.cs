namespace temalab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Items", new[] { "CategoryID" });
            AlterColumn("dbo.Items", "CategoryID", c => c.Int());
            CreateIndex("dbo.Items", "CategoryID");
            AddForeignKey("dbo.Items", "CategoryID", "dbo.Categories", "CategoryID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Items", new[] { "CategoryID" });
            AlterColumn("dbo.Items", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "CategoryID");
            AddForeignKey("dbo.Items", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
        }
    }
}
