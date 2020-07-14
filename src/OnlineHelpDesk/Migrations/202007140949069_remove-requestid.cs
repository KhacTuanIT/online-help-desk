namespace OnlineHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerequestid : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.RequestStatus", name: "RequestId", newName: "Request_Id");
            RenameIndex(table: "dbo.RequestStatus", name: "IX_RequestId", newName: "IX_Request_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.RequestStatus", name: "IX_Request_Id", newName: "IX_RequestId");
            RenameColumn(table: "dbo.RequestStatus", name: "Request_Id", newName: "RequestId");
        }
    }
}
