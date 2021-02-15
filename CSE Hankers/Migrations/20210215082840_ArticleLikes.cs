using Microsoft.EntityFrameworkCore.Migrations;

namespace CSE_Hankers.Migrations
{
    public partial class ArticleLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleLikes",
                columns: table => new
                {
                    articleLikeaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    articleId = table.Column<int>(type: "int", nullable: true),
                    authorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleLikes", x => x.articleLikeaId);
                    table.ForeignKey(
                        name: "FK_ArticleLikes_Articles_articleId",
                        column: x => x.articleId,
                        principalTable: "Articles",
                        principalColumn: "articleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleLikes_AspNetUsers_authorId",
                        column: x => x.authorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLikes_articleId",
                table: "ArticleLikes",
                column: "articleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLikes_authorId",
                table: "ArticleLikes",
                column: "authorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleLikes");
        }
    }
}
