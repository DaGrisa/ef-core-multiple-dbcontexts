using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectTwo.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "projectTwo");

            migrationBuilder.CreateTable(
                name: "ClassTwo",
                schema: "projectTwo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTwo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassTwo",
                schema: "projectTwo");
        }
    }
}
