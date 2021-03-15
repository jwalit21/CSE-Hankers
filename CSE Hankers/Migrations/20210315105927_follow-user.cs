using Microsoft.EntityFrameworkCore.Migrations;

namespace CSE_Hankers.Migrations
{
    public partial class followuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleComments_Articles_articleId",
                table: "ArticleComments");

            migrationBuilder.AlterColumn<int>(
                name: "articleId",
                table: "ArticleComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserFollowings",
                columns: table => new
                {
                    userFollowingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    followerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowings", x => x.userFollowingId);
                    table.ForeignKey(
                        name: "FK_UserFollowings_AspNetUsers_followerId",
                        column: x => x.followerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFollowings_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowings_followerId",
                table: "UserFollowings",
                column: "followerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowings_userId",
                table: "UserFollowings",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleComments_Articles_articleId",
                table: "ArticleComments",
                column: "articleId",
                principalTable: "Articles",
                principalColumn: "articleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleComments_Articles_articleId",
                table: "ArticleComments");

            migrationBuilder.DropTable(
                name: "UserFollowings");

            migrationBuilder.AlterColumn<int>(
                name: "articleId",
                table: "ArticleComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleComments_Articles_articleId",
                table: "ArticleComments",
                column: "articleId",
                principalTable: "Articles",
                principalColumn: "articleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
