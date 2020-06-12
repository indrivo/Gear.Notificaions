using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gear.Notifications.Infrastructure.Persistance.Migrations
{
    public partial class InitialConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventName = table.Column<string>(maxLength: 50, nullable: false),
                    HtmlEventMarkupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HtmlEventMarkups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Subtitle = table.Column<string>(nullable: false),
                    ChangesMarkup = table.Column<string>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlEventMarkups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HtmlEventMarkups_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEvents",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    NotificationProfileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEvents", x => new { x.EventId, x.NotificationProfileId });
                    table.ForeignKey(
                        name: "FK_NotificationEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationEvents_NotificationProfiles_NotificationProfileId",
                        column: x => x.NotificationProfileId,
                        principalTable: "NotificationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationUsers",
                columns: table => new
                {
                    NotificationProfileId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUsers", x => new { x.UserId, x.NotificationProfileId });
                    table.ForeignKey(
                        name: "FK_NotificationUsers_NotificationProfiles_NotificationProfileId",
                        column: x => x.NotificationProfileId,
                        principalTable: "NotificationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HtmlEventMarkups_EventId",
                table: "HtmlEventMarkups",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEvents_NotificationProfileId",
                table: "NotificationEvents",
                column: "NotificationProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUsers_NotificationProfileId",
                table: "NotificationUsers",
                column: "NotificationProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HtmlEventMarkups");

            migrationBuilder.DropTable(
                name: "NotificationEvents");

            migrationBuilder.DropTable(
                name: "NotificationUsers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "NotificationProfiles");
        }
    }
}
