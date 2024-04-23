using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JevKrayPersonalSite.Migrations
{
    /// <inheritdoc />
    public partial class initialprojects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPictures_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Commits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTimeOffset(new DateTime(2024, 3, 22, 13, 30, 9, 586, DateTimeKind.Unspecified).AddTicks(2919), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPictures_ProjectId",
                table: "ProjectPictures",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPictures");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.UpdateData(
                table: "Commits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTimeOffset(new DateTime(2024, 2, 17, 6, 33, 20, 879, DateTimeKind.Unspecified).AddTicks(774), new TimeSpan(0, 3, 0, 0, 0)));
        }
    }
}
