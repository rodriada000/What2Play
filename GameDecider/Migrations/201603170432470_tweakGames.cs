namespace GameDecider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tweakGames : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.VideoGames", name: "ApplicationUser_Id", newName: "UserID");
            RenameIndex(table: "dbo.VideoGames", name: "IX_ApplicationUser_Id", newName: "IX_UserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.VideoGames", name: "IX_UserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.VideoGames", name: "UserID", newName: "ApplicationUser_Id");
        }
    }
}
