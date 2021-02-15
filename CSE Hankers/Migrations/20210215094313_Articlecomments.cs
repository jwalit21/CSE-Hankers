using Microsoft.EntityFrameworkCore.Migrations;

namespace CSE_Hankers.Migrations
{
    public partial class Articlecomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "articleId",
                table: "ArticleComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_articleId",
                table: "ArticleComments",
                column: "articleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleComments_Articles_articleId",
                table: "ArticleComments",
                column: "articleId",
                principalTable: "Articles",
                principalColumn: "articleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleComments_Articles_articleId",
                table: "ArticleComments");

            migrationBuilder.DropIndex(
                name: "IX_ArticleComments_articleId",
                table: "ArticleComments");

            migrationBuilder.DropColumn(
                name: "articleId",
                table: "ArticleComments");
        }
    }
}
