using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 12, 11, 8, 3, 290, DateTimeKind.Utc).AddTicks(8939), "zJhROOnhN4tgw1Jj0Qq6+eLVIx8W/JO3v49c8SUxFS3yboihi0lHz92QdP+lcTp2UehbC8ZDmRsfJbGIFAc4tA==:93qwNPWz8+r5kilnVFYKDCibm8OCUPRflFjXcFA8IO8=" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { 1, "admin@example.com", "Admin", "User", "Yyi0YriHKiNIgOMRZC7sYmMDKdKYTdypCmPBB+1l/YHEXKCsRnUZ6g0JQ9R9cdjd9bT/U0+96pPtSWgLNCZAGA==:dIge5JctVjdPcDZZnTbXO2zTYqSJiO06XSZoprU1uwY=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 12, 10, 13, 8, 408, DateTimeKind.Utc).AddTicks(1170), "WKg/IJptaJ0EZbq1PNm64d722zKFES6i2NyuemYNUN40vDAPaS51TlyjFzSJV7L4HkMZOBHbtHTsTY7HBDaLaw==:+NtkV9OWLtboKLiI/yVHvadETWpeRwQOP2Fa6DnCN0I=" });
        }
    }
}
