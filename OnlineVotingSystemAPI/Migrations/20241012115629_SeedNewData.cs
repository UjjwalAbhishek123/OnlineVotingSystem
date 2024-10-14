using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 12, 11, 56, 28, 681, DateTimeKind.Utc).AddTicks(3581), "wuN82nP57XCeBYnDqaab9RCoTP7VaDz8UQfaJQ9idR+aF+91Pt5+HxwzE9KTK4okvUvoTPim0JAsXUm8C5d1mw==:g3lAaIoNOu0HlXHTF19PSEvYjwI8ChwgLWBRkLWJaDQ=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "Tl6dHSukh8ZwGgG0spd5YiZX0tQd2BWy39+YAUADvzHHNxpuUTxoLdIlxf3QqSKhhWN8je4s+gye6hyGUDUbPA==:b2din1aLef/gjFb7KdFZonOx+tSh2bByyDm/5y10bLk=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
