using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVotingSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreatedCandidateTableAndVotingEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VotingEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Party = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VotingEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_VotingEvents_VotingEventId",
                        column: x => x.VotingEventId,
                        principalTable: "VotingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2024, 10, 14, 6, 50, 39, 393, DateTimeKind.Utc).AddTicks(2917), "vh82WhmTD/Db0OSj8KysR/qqkQyWX8IcNxT0fbm/LGQvVXe8p28XUrD4POMdu+BKGdPuvosS129IIFGRY6e94Q==:oWMdvhn0r89hi6qfoNkhSiREbMrE5ZU0Hu2EesQgRCI=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ht4Bk1j4pLA7f63/r/LjZ9zWcEZF2PupF0Kn2Ax/HkUxPquzIsR+MGV4WEUzDNNCao6mKtK7Tc3qdb7PPxq+DQ==:Krmd+ra9yAdAcTtD6FZ+QayX5GQE6AmE7F+7KiylpKA=");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_VotingEventId",
                table: "Candidates",
                column: "VotingEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "VotingEvents");

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
    }
}
