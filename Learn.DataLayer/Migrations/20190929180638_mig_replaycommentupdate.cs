using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_replaycommentupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_CourseComments_CourseCommentCommentId",
                table: "CourseComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_CourseCommentCommentId",
                table: "CourseComments");

            migrationBuilder.DropColumn(
                name: "CourseCommentCommentId",
                table: "CourseComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseCommentCommentId",
                table: "CourseComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_CourseCommentCommentId",
                table: "CourseComments",
                column: "CourseCommentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_CourseComments_CourseCommentCommentId",
                table: "CourseComments",
                column: "CourseCommentCommentId",
                principalTable: "CourseComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
