namespace GameDecider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlatformName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Platforms",
                c => new
                    {
                        PlatformID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PlatformID);
            
            AddColumn("dbo.VideoGames", "PlatformName_PlatformID", c => c.Int());
            CreateIndex("dbo.VideoGames", "PlatformName_PlatformID");
            AddForeignKey("dbo.VideoGames", "PlatformName_PlatformID", "dbo.Platforms", "PlatformID");
            DropColumn("dbo.VideoGames", "PlatformID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VideoGames", "PlatformID", c => c.Int(nullable: false));
            DropForeignKey("dbo.VideoGames", "PlatformName_PlatformID", "dbo.Platforms");
            DropIndex("dbo.VideoGames", new[] { "PlatformName_PlatformID" });
            DropColumn("dbo.VideoGames", "PlatformName_PlatformID");
            DropTable("dbo.Platforms");
        }
    }
}
