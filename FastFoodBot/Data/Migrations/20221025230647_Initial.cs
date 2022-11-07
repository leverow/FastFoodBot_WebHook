using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Telegram.Bot.Examples.WebHook.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Step = table.Column<ushort>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
