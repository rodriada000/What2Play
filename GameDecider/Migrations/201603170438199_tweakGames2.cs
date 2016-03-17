namespace GameDecider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tweakGames2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VideoGames", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.VideoGames", new[] { "UserID" });
            AddColumn("dbo.VideoGames", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.VideoGames", "UserID", c => c.String());
            CreateIndex("dbo.VideoGames", "ApplicationUser_Id");
            AddForeignKey("dbo.VideoGames", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoGames", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.VideoGames", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.VideoGames", "UserID", c => c.String(maxLength: 128));
            DropColumn("dbo.VideoGames", "ApplicationUser_Id");
            CreateIndex("dbo.VideoGames", "UserID");
            AddForeignKey("dbo.VideoGames", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
