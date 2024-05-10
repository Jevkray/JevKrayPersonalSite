using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JevKrayPersonalSite.Migrations
{
    /// <inheritdoc />
    public partial class updateimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "ProjectPictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Commits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTimeOffset(new DateTime(2024, 3, 22, 14, 45, 38, 638, DateTimeKind.Unspecified).AddTicks(200), new TimeSpan(0, 3, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "ProjectPictures");

            migrationBuilder.UpdateData(
                table: "Commits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTimeOffset(new DateTime(2024, 3, 22, 13, 30, 9, 586, DateTimeKind.Unspecified).AddTicks(2919), new TimeSpan(0, 3, 0, 0, 0)));
        }
    }
}
