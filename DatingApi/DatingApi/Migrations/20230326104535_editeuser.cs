using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class editeuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KnowsAs",
                table: "AppUser",
                newName: "KnownAs");

            migrationBuilder.RenameColumn(
                name: "Intersets",
                table: "AppUser",
                newName: "Interests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KnownAs",
                table: "AppUser",
                newName: "KnowsAs");

            migrationBuilder.RenameColumn(
                name: "Interests",
                table: "AppUser",
                newName: "Intersets");
        }
    }
}
