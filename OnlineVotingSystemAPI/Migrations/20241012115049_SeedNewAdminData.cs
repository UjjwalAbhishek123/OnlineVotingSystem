using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 12, 11, 50, 47, 840, DateTimeKind.Utc).AddTicks(7377), "4TjdKqa5R0y/cGtDlZVnOQXXekHBYh5hzExQ1cSkMDY74ePByPK2Ki4IAlxdSVM4uQX5+zAN7WEEFb1p/36kNQ==:tOqJvNL0Bte90FPSY9djl8NiKhWHp07hqUw8neQn5qw=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "4g3NCIOsGXydXvY0EhymvfOBu8I2IgkH3fFwvnVRD43uZhlQNdsG9bvszJDO0V9ExtMMpMUrvblxlaxXwHGCXA==:MEQH06E711Th4f6WTX+1Lb/eWQjetr81pSzIAgtMHBU=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 12, 11, 8, 3, 290, DateTimeKind.Utc).AddTicks(8939), "zJhROOnhN4tgw1Jj0Qq6+eLVIx8W/JO3v49c8SUxFS3yboihi0lHz92QdP+lcTp2UehbC8ZDmRsfJbGIFAc4tA==:93qwNPWz8+r5kilnVFYKDCibm8OCUPRflFjXcFA8IO8=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "Yyi0YriHKiNIgOMRZC7sYmMDKdKYTdypCmPBB+1l/YHEXKCsRnUZ6g0JQ9R9cdjd9bT/U0+96pPtSWgLNCZAGA==:dIge5JctVjdPcDZZnTbXO2zTYqSJiO06XSZoprU1uwY=");
        }
    }
}
