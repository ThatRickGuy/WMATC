namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Events",
            //    c => new
            //        {
            //            EventId = c.Int(nullable: false, identity: true),
            //            Title = c.String(),
            //            EventDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Factions",
                c => new
                    {
                        FactionId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImageURL = c.String(),
                    })
                .PrimaryKey(t => t.FactionId);
            
            //CreateTable(
            //    "dbo.Matchups",
            //    c => new
            //        {
            //            MatchupId = c.Int(nullable: false, identity: true),
            //            RoundTeamMatchupId = c.Int(nullable: false),
            //            Player1Id = c.Int(nullable: false),
            //            Player2Id = c.Int(nullable: false),
            //            WinnerId = c.Int(),
            //            Player1List = c.Int(nullable: false),
            //            Player2List = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.MatchupId)
            //    .ForeignKey("dbo.Players", t => t.Player1Id, cascadeDelete: true)
            //    .ForeignKey("dbo.Players", t => t.Player2Id, cascadeDelete: true)
            //    .ForeignKey("dbo.RoundTeamMatchups", t => t.RoundTeamMatchupId, cascadeDelete: true)
            //    .ForeignKey("dbo.Players", t => t.WinnerId)
            //    .Index(t => t.RoundTeamMatchupId)
            //    .Index(t => t.Player1Id)
            //    .Index(t => t.Player2Id)
            //    .Index(t => t.WinnerId);
            
            //CreateTable(
            //    "dbo.Players",
            //    c => new
            //        {
            //            PlayerId = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            ImgURL = c.String(),
            //            List1 = c.String(),
            //            List2 = c.String(),
            //            TeamId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PlayerId)
            //    .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
            //    .Index(t => t.TeamId);
            
            //CreateTable(
            //    "dbo.Teams",
            //    c => new
            //        {
            //            TeamId = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            ImgURL = c.String(),
            //            EventId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.TeamId)
            //    .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
            //    .Index(t => t.EventId);
            
            //CreateTable(
            //    "dbo.RoundTeamMatchups",
            //    c => new
            //        {
            //            RoundTeamMatchupId = c.Int(nullable: false, identity: true),
            //            RoundId = c.Int(nullable: false),
            //            Team1Id = c.Int(nullable: false),
            //            Team2Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.RoundTeamMatchupId)
            //    .ForeignKey("dbo.Rounds", t => t.RoundId, cascadeDelete: true)
            //    .ForeignKey("dbo.Teams", t => t.Team1Id, cascadeDelete: true)
            //    .ForeignKey("dbo.Teams", t => t.Team2Id, cascadeDelete: true)
            //    .Index(t => t.RoundId)
            //    .Index(t => t.Team1Id)
            //    .Index(t => t.Team2Id);
            
            //CreateTable(
            //    "dbo.Rounds",
            //    c => new
            //        {
            //            RoundId = c.Int(nullable: false, identity: true),
            //            Sequence = c.Int(nullable: false),
            //            Scenario = c.String(),
            //            EventId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.RoundId)
            //    .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
            //    .Index(t => t.EventId);
            
            //CreateTable(
            //    "dbo.AspNetRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Name = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserRoles",
            //    c => new
            //        {
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            RoleId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.UserId, t.RoleId })
            //    .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId)
            //    .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.AspNetUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Email = c.String(maxLength: 256),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            //CreateTable(
            //    "dbo.AspNetUserClaims",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            ClaimType = c.String(),
            //            ClaimValue = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.AspNetUserLogins",
            //    c => new
            //        {
            //            LoginProvider = c.String(nullable: false, maxLength: 128),
            //            ProviderKey = c.String(nullable: false, maxLength: 128),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Matchups", "WinnerId", "dbo.Players");
            DropForeignKey("dbo.Matchups", "RoundTeamMatchupId", "dbo.RoundTeamMatchups");
            DropForeignKey("dbo.RoundTeamMatchups", "Team2Id", "dbo.Teams");
            DropForeignKey("dbo.RoundTeamMatchups", "Team1Id", "dbo.Teams");
            DropForeignKey("dbo.RoundTeamMatchups", "RoundId", "dbo.Rounds");
            DropForeignKey("dbo.Rounds", "EventId", "dbo.Events");
            DropForeignKey("dbo.Matchups", "Player2Id", "dbo.Players");
            DropForeignKey("dbo.Matchups", "Player1Id", "dbo.Players");
            DropForeignKey("dbo.Players", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Teams", "EventId", "dbo.Events");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Rounds", new[] { "EventId" });
            DropIndex("dbo.RoundTeamMatchups", new[] { "Team2Id" });
            DropIndex("dbo.RoundTeamMatchups", new[] { "Team1Id" });
            DropIndex("dbo.RoundTeamMatchups", new[] { "RoundId" });
            DropIndex("dbo.Teams", new[] { "EventId" });
            DropIndex("dbo.Players", new[] { "TeamId" });
            DropIndex("dbo.Matchups", new[] { "WinnerId" });
            DropIndex("dbo.Matchups", new[] { "Player2Id" });
            DropIndex("dbo.Matchups", new[] { "Player1Id" });
            DropIndex("dbo.Matchups", new[] { "RoundTeamMatchupId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Rounds");
            DropTable("dbo.RoundTeamMatchups");
            DropTable("dbo.Teams");
            DropTable("dbo.Players");
            DropTable("dbo.Matchups");
            DropTable("dbo.Factions");
            DropTable("dbo.Events");
        }
    }
}
