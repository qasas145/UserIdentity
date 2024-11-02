using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserIdentity.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table : "Roles",
                columns : new[] {"Id", "Name", "NormalizedName", "ConcurrencyStamp"},
                values : new object[] {Guid.NewGuid().ToString(), "User", "User".ToUpper(), Guid.NewGuid().ToString()},
                schema : "Muhqasas"
            );
            migrationBuilder.InsertData(
                table : "Roles",
                columns : new[] {"Id", "Name", "NormalizedName", "ConcurrencyStamp"},
                values : new object[] {Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString()},
                schema : "Muhqasas"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete  from [Muhqasas].[Roles]");
            
        }
    }
}
