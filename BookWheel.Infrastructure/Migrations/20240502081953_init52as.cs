using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWheel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init52as : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_LocationId",
                table: "Ratings",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Location_LocationId",
                table: "Ratings",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Location_LocationId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_LocationId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Ratings");
        }
    }
}
