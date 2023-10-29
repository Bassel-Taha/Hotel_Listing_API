using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP_API_Udemy_Course.Migrations
{
    /// <inheritdoc />
    public partial class Addin_Data_To_IdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "881c4cbd-989b-499c-9118-6b542af4a81e", null, "ADMIN", "Admin" },
                    { "8ee60f2b-8257-4c68-91f9-9cb34789d707", null, "USER1", "User1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "881c4cbd-989b-499c-9118-6b542af4a81e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ee60f2b-8257-4c68-91f9-9cb34789d707");
        }
    }
}
