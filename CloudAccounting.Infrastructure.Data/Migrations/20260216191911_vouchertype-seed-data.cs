#pragma warning disable CS8981

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CloudAccounting.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class vouchertypeseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GL_VOUCHER",
                columns: new[] { "VCHCODE", "VCHNATURE", "VCHTITLE", "VCHTYPE" },
                values: new object[,]
                {
                    { 1, (byte)1, "Bank Payment Voucher", "BPV" },
                    { 2, (byte)3, "Local Sales Invoice", "LSI" },
                    { 3, (byte)2, "Bank Receipt Voucher", "BRV" },
                    { 4, (byte)3, "Adjustment Voucher", "ADJ" },
                    { 5, (byte)1, "Purchase Order", "PO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GL_VOUCHER",
                keyColumn: "VCHCODE",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GL_VOUCHER",
                keyColumn: "VCHCODE",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GL_VOUCHER",
                keyColumn: "VCHCODE",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GL_VOUCHER",
                keyColumn: "VCHCODE",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GL_VOUCHER",
                keyColumn: "VCHCODE",
                keyValue: 5);
        }
    }
}
