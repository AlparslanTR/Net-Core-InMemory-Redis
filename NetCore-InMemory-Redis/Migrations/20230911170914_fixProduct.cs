using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCore_InMemory_Redis.Migrations
{
    /// <inheritdoc />
    public partial class fixProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "decimal(18,2)",
                table: "Products",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "decimal(18,2)");
        }
    }
}
