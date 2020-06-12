using Microsoft.EntityFrameworkCore.Migrations;

namespace Gear.Notifications.Infrastructure.Persistance.Migrations
{
    public partial class GroupName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryEntityGroupName",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryEntityGroupName",
                table: "Notifications");
        }
    }
}
