using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class ignoring_phoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "Muhqasas",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "Muhqasas",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
