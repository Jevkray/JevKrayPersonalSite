using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JevKrayPersonalSite.Migrations
{
    /// <inheritdoc />
    public partial class updateprojectsaddpreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPreview",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Commits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTimeOffset(new DateTime(2024, 3, 23, 11, 1, 16, 49, DateTimeKind.Unspecified).AddTicks(7289), new TimeSpan(0, 3, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPreview",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Commits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTimeOffset(new DateTime(2024, 3, 22, 14, 45, 38, 638, DateTimeKind.Unspecified).AddTicks(200), new TimeSpan(0, 3, 0, 0, 0)));
        }
    }
}
