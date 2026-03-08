#pragma warning disable CS8981

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CloudAccounting.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class companyseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GL_COMPANY",
                columns: new[] { "COCODE", "COADDRESS", "COCITY", "CONAME", "COCURRENCY", "COFAX", "COPHONE", "COZIP" },
                values: new object[,]
                {
                    { 1, "123 Main Street", "Dallas", "BTechnical Consulting", "USD", "214-555-5678", "214-555-1234", "75201" },
                    { 2, "456 Elm Street", "Seattle", "Contoso Ltd.", "USD", "206-555-5432", "206-555-9876", "98101" },
                    { 3, "789 Oak Avenue", "Chicago", "Fabrikam Inc.", "USD", "312-555-1357", "312-555-2468", "60601" },
                    { 4, "321 Pine Road", "Denver", "Adventure Works", "USD", "303-555-4321", "303-555-7890", "80201" },
                    { 5, "654 Maple Lane", "Boston", "Contoso Pharmaceuticals", "USD", "617-555-1357", "617-555-2468", "02101" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GL_COMPANY",
                keyColumn: "COCODE",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GL_COMPANY",
                keyColumn: "COCODE",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GL_COMPANY",
                keyColumn: "COCODE",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GL_COMPANY",
                keyColumn: "COCODE",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GL_COMPANY",
                keyColumn: "COCODE",
                keyValue: 5);
        }
    }
}
