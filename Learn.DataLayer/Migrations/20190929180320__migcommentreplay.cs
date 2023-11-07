using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class _migcommentreplay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_CourseComments_ParentId",
                table: "CourseComments");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "CourseComments",
                newName: "CourseCommentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseComments_ParentId",
                table: "CourseComments",
                newName: "IX_CourseComments_CourseCommentCommentId");

            migrationBuilder.CreateTable(
                name: "ReplayCourseComments",
                columns: table => new
                {
                    ReplayCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(maxLength: 700, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    IsAdminRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayCourseComments", x => x.ReplayCommentId);
                    table.ForeignKey(
                        name: "FK_ReplayCourseComments_CourseComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CourseComments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplayCourseComments_CommentId",
                table: "ReplayCourseComments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_CourseComments_CourseCommentCommentId",
                table: "CourseComments",
                column: "CourseCommentCommentId",
                principalTable: "CourseComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_CourseComments_CourseCommentCommentId",
                table: "CourseComments");

            migrationBuilder.DropTable(
                name: "ReplayCourseComments");

            migrationBuilder.RenameColumn(
                name: "CourseCommentCommentId",
                table: "CourseComments",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseComments_CourseCommentCommentId",
                table: "CourseComments",
                newName: "IX_CourseComments_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_CourseComments_ParentId",
                table: "CourseComments",
                column: "ParentId",
                principalTable: "CourseComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
