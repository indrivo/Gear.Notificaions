using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gear.Notifications.Infrastructure.Persistance.Migrations
{
    public partial class NotificationsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationTypes",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropagationTypes",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Users = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    NotificationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationTypes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PropagationTypes",
                table: "Events");
        }
    }
}
