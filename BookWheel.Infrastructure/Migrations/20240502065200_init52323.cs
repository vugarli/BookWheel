using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWheel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init52323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingRoot",
                table: "RatingRoot");

            migrationBuilder.RenameTable(
                name: "RatingRoot",
                newName: "Ratings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "RatingRoot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingRoot",
                table: "RatingRoot",
                column: "Id");
        }
    }
}
