using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CloudAccounting.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FiscalYearSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GL_FISCAL_YEAR",
                columns: new[] { "COCODE", "COMONTHID", "COYEAR", "COMONTHNAME", "INITIAL_YEAR", "MONTH_CLOSED", "PFROM", "PTO", "TYE_EXECUTED", "YEAR_CLOSED" },
                values: new object[,]
                {
                    { 1, (byte)1, (short)2024, "January", true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)2, (short)2024, "February", true, false, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)3, (short)2024, "March", true, false, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)4, (short)2024, "April", true, false, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)5, (short)2024, "May", true, false, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)6, (short)2024, "June", true, false, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)7, (short)2024, "July", true, false, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)8, (short)2024, "August", true, false, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)9, (short)2024, "September", true, false, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)10, (short)2024, "October", true, false, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)11, (short)2024, "November", true, false, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)12, (short)2024, "December", true, false, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)1, (short)2025, "January", true, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)2, (short)2025, "February", true, false, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)3, (short)2025, "March", true, false, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)4, (short)2025, "April", true, false, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)5, (short)2025, "May", true, false, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)6, (short)2025, "June", true, false, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)7, (short)2025, "July", true, false, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)8, (short)2025, "August", true, false, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)9, (short)2025, "September", true, false, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)10, (short)2025, "October", true, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)11, (short)2025, "November", true, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { 1, (byte)12, (short)2025, "December", true, false, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)1, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)2, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)3, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)4, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)5, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)6, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)7, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)8, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)9, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)10, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)11, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)12, (short)2024 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)1, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)2, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)3, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)4, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)5, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)6, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)7, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)8, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)9, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)10, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)11, (short)2025 });

            migrationBuilder.DeleteData(
                table: "GL_FISCAL_YEAR",
                keyColumns: new[] { "COCODE", "COMONTHID", "COYEAR" },
                keyValues: new object[] { 1, (byte)12, (short)2025 });
        }
    }
}
