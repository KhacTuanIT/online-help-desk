namespace OnlineHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacilityId = c.Int(),
                        ArtifactId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EquipmentType", t => t.ArtifactId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .Index(t => t.FacilityId)
                .Index(t => t.ArtifactId);
            
            CreateTable(
                "dbo.EquipmentType",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        artifact_name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Facility",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        CreatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FacilityHead",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        FacilityId = c.Int(),
                        PositionName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Facility", t => t.FacilityId)
                .Index(t => t.UserId)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserIdentityCode = c.String(maxLength: 32),
                        FullName = c.String(nullable: false, maxLength: 255),
                        Address = c.String(),
                        Avatar = c.String(),
                        CreatedAt = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Message = c.String(nullable: false),
                        Seen = c.Boolean(),
                        CreatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Report",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Resource = c.String(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        CreatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PetitionerId = c.String(maxLength: 128),
                        AssignedHeadId = c.Int(),
                        EquipmentId = c.Int(),
                        RequestTypeId = c.Int(),
                        RequestStatusId = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FacilityHead", t => t.AssignedHeadId)
                .ForeignKey("dbo.RequestType", t => t.RequestTypeId)
                .ForeignKey("dbo.AspNetUsers", t => t.PetitionerId)
                .ForeignKey("dbo.Equipment", t => t.EquipmentId)
                .Index(t => t.PetitionerId)
                .Index(t => t.AssignedHeadId)
                .Index(t => t.EquipmentId)
                .Index(t => t.RequestTypeId);
            
            CreateTable(
                "dbo.RequestStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(),
                        StatusTypeId = c.Int(),
                        Message = c.String(nullable: false),
                        TimeCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StatusType", t => t.StatusTypeId)
                .ForeignKey("dbo.Request", t => t.RequestId)
                .Index(t => t.RequestId)
                .Index(t => t.StatusTypeId);
            
            CreateTable(
                "dbo.StatusType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Request", "EquipmentId", "dbo.Equipment");
            DropForeignKey("dbo.FacilityHead", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Request", "PetitionerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Request", "RequestTypeId", "dbo.RequestType");
            DropForeignKey("dbo.RequestStatus", "RequestId", "dbo.Request");
            DropForeignKey("dbo.RequestStatus", "StatusTypeId", "dbo.StatusType");
            DropForeignKey("dbo.Request", "AssignedHeadId", "dbo.FacilityHead");
            DropForeignKey("dbo.Report", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notification", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FacilityHead", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Equipment", "FacilityId", "dbo.Facility");
            DropForeignKey("dbo.Equipment", "ArtifactId", "dbo.EquipmentType");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.RequestStatus", new[] { "StatusTypeId" });
            DropIndex("dbo.RequestStatus", new[] { "RequestId" });
            DropIndex("dbo.Request", new[] { "RequestTypeId" });
            DropIndex("dbo.Request", new[] { "EquipmentId" });
            DropIndex("dbo.Request", new[] { "AssignedHeadId" });
            DropIndex("dbo.Request", new[] { "PetitionerId" });
            DropIndex("dbo.Report", new[] { "CreatorId" });
            DropIndex("dbo.Notification", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.FacilityHead", new[] { "FacilityId" });
            DropIndex("dbo.FacilityHead", new[] { "UserId" });
            DropIndex("dbo.Equipment", new[] { "ArtifactId" });
            DropIndex("dbo.Equipment", new[] { "FacilityId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.RequestType");
            DropTable("dbo.StatusType");
            DropTable("dbo.RequestStatus");
            DropTable("dbo.Request");
            DropTable("dbo.Report");
            DropTable("dbo.Notification");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.FacilityHead");
            DropTable("dbo.Facility");
            DropTable("dbo.EquipmentType");
            DropTable("dbo.Equipment");
        }
    }
}
