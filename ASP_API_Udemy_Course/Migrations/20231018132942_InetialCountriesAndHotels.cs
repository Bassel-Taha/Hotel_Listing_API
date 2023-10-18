using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP_API_Udemy_Course.Migrations
{
    /// <inheritdoc />
    public partial class InetialCountriesAndHotels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "egypt", "EG" },
                    { 2, "Saudia arabia", "SAR" },
                    { 3, "United Arab Imarates", "UAE" }
                });

            migrationBuilder.InsertData(
                table: "hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "MAKA", 2, "the clock tower", 5 },
                    { 2, "Dubai", 3, "Burj Khalifa", 5 },
                    { 3, "Alexandrai", 1, "The Four Seasoons", 4 },
                    { 4, "Almadina Almonawara", 2, "Almadina Hotel", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "hotels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
