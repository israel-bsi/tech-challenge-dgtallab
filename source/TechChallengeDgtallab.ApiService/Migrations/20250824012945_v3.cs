using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechChallengeDgtallab.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentHierarchies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentHierarchies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SuperiorDepartmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });
        }
    }
}
