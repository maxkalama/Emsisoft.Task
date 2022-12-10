using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emsisoft.DB.Migrations
{
    /// <inheritdoc />
    public partial class index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Hashes_Date",
                table: "Hashes",
                column: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hashes_Date",
                table: "Hashes");
        }
    }
}
