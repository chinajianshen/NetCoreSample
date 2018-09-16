using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NineskyStudy.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Target = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGeneral",
                columns: table => new
                {
                    GeneralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    View = table.Column<string>(maxLength: 200, nullable: false),
                    Module = table.Column<string>(maxLength: 50, nullable: false),
                    ContentView = table.Column<string>(maxLength: 200, nullable: false),
                    ContentOrder = table.Column<int>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGeneral", x => x.GeneralId);
                    table.ForeignKey(
                        name: "FK_CategoryGeneral_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryLink",
                columns: table => new
                {
                    LinkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLink", x => x.LinkId);
                    table.ForeignKey(
                        name: "FK_CategoryLink_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPage",
                columns: table => new
                {
                    PageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 10000, nullable: false),
                    View = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPage", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_CategoryPage_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGeneral_CategoryId",
                table: "CategoryGeneral",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLink_CategoryId",
                table: "CategoryLink",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPage_CategoryId",
                table: "CategoryPage",
                column: "CategoryId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryGeneral");

            migrationBuilder.DropTable(
                name: "CategoryLink");

            migrationBuilder.DropTable(
                name: "CategoryPage");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
