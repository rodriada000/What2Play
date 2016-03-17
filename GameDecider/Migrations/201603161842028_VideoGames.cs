namespace GameDecider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoGames : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoGames",
                c => new
                    {
                        VideoGameID = c.Int(nullable: false, identity: true),
                        GameID = c.Int(nullable: false),
                        PlatformID = c.Int(nullable: false),
                        Favorite = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VideoGameID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoGames", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.VideoGames", new[] { "ApplicationUser_Id" });
            DropTable("dbo.VideoGames");
        }
    }
}
