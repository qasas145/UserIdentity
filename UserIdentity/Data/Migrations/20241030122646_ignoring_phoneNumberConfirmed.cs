using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class ignoring_phoneNumberConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "Muhqasas",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "Muhqasas",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
