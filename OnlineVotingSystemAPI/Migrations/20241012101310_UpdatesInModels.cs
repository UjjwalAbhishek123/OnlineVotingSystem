using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesInModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Admins");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Admins",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { 1, new DateTime(2024, 10, 12, 10, 13, 8, 408, DateTimeKind.Utc).AddTicks(1170), "admin@example.com", "Admin", "User", "WKg/IJptaJ0EZbq1PNm64d722zKFES6i2NyuemYNUN40vDAPaS51TlyjFzSJV7L4HkMZOBHbtHTsTY7HBDaLaw==:+NtkV9OWLtboKLiI/yVHvadETWpeRwQOP2Fa6DnCN0I=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Admins");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
