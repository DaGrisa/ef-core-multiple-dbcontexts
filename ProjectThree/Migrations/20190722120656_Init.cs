using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectThree.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "projectThree");

            migrationBuilder.CreateTable(
                name: "ClassThree",
                schema: "projectThree",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassThree", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassThree",
                schema: "projectThree");
        }
    }
}
