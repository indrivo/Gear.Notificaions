using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gear.Notifications.Infrastructure.Persistance.Migrations
{
    public partial class eventAdditionalConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryEntityGroup",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedirectAction",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryEntityGroup",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RedirectAction",
                table: "Notifications");
        }
    }
}
