using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConverterTelegramBot.Migrations
{
    /// <inheritdoc />
    public partial class AddLastDocumentToBotUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastDocument",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDocument",
                table: "Users");
        }
    }
}
