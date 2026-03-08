using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudAccounting.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class IndexCompanyVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GL_VOUCHER_VCHTYPE",
                table: "GL_VOUCHER",
                column: "VCHTYPE",
                unique: true,
                filter: "[VCHTYPE] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GL_COMPANY_CONAME",
                table: "GL_COMPANY",
                column: "CONAME",
                unique: true,
                filter: "[CONAME] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GL_VOUCHER_VCHTYPE",
                table: "GL_VOUCHER");

            migrationBuilder.DropIndex(
                name: "IX_GL_COMPANY_CONAME",
                table: "GL_COMPANY");
        }
    }
}
