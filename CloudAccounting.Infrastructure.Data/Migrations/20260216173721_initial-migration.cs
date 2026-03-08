#pragma warning disable CS8981

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudAccounting.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GL_BUDGET_REPORT",
                columns: table => new
                {
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    COATITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BUDGET = table.Column<decimal>(type: "MONEY", nullable: true),
                    ACTUAL = table.Column<decimal>(type: "MONEY", nullable: true),
                    VARIANCE = table.Column<decimal>(type: "MONEY", nullable: true),
                    PERCENT = table.Column<decimal>(type: "DECIMAL(7,2)", nullable: true),
                    STATUS = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    USERID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    GRAND_TOTAL = table.Column<bool>(type: "BIT", nullable: true),
                    CONAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ACCOUNTFROM = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    ACCOUNTTO = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    MONTHFROM = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    MONTHTO = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    PRINTEDON = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GL_COMPANY",
                columns: table => new
                {
                    COCODE = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CONAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    COADDRESS = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    COPHONE = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    COFAX = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    COCITY = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    COZIP = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    COCURRENCY = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_COMPANY_PK", x => x.COCODE);
                });

            migrationBuilder.CreateTable(
                name: "GL_DASHBOARD",
                columns: table => new
                {
                    SRNO = table.Column<int>(type: "INT", nullable: true),
                    ACCOUNTTITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CURRENTYEAR = table.Column<decimal>(type: "MONEY", nullable: true),
                    PREVIOUSYEAR = table.Column<decimal>(type: "MONEY", nullable: true),
                    USERID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RATIOTITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CURRENT_YEAR = table.Column<decimal>(type: "MONEY", nullable: true),
                    PREVIOUS_YEAR = table.Column<decimal>(type: "MONEY", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GL_FEEDBACK",
                columns: table => new
                {
                    FEEDBACKID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TS = table.Column<DateTime>(type: "DATETIME2", nullable: true, defaultValueSql: "GETDATE()"),
                    CUSTNAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CUSTEMAIL = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CUSTFEEDBACK = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_FEEDBACK_PK", x => x.FEEDBACKID);
                });

            migrationBuilder.CreateTable(
                name: "GL_FS_REPORT",
                columns: table => new
                {
                    REPORTCODE = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    REPORTTITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SRNO = table.Column<int>(type: "INT", nullable: true),
                    FSACCOUNT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CURRENTBALANCE = table.Column<decimal>(type: "MONEY", nullable: true),
                    PREVIOUSBALANCE = table.Column<decimal>(type: "MONEY", nullable: true),
                    PERCENT = table.Column<decimal>(type: "DECIMAL(7,2)", nullable: true),
                    USERID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CONAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    COYEAR = table.Column<short>(type: "SMALLINT", nullable: true),
                    COMONTHNAME = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    CALCULATION = table.Column<bool>(type: "BIT", nullable: true),
                    NETVALUE = table.Column<bool>(type: "BIT", nullable: true),
                    NOTES = table.Column<bool>(type: "BIT", nullable: true),
                    NOTESCODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    NOTESTITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    HEADING = table.Column<bool>(type: "BIT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GL_FS_SETUP",
                columns: table => new
                {
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    REPORTCODE = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    FSACCOUNT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    REPORTTITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ACCOUNTFROM = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    ACCOUNTTO = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_FS_SETUP_PK", x => new { x.COCODE, x.REPORTCODE, x.FSACCOUNT });
                });

            migrationBuilder.CreateTable(
                name: "GL_GROUPS_MASTER",
                columns: table => new
                {
                    GROUPID = table.Column<short>(type: "SMALLINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GROUPTITLE = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_GROUPS_PK", x => x.GROUPID);
                });

            migrationBuilder.CreateTable(
                name: "GL_RECONCILE_REPORT",
                columns: table => new
                {
                    SRNO = table.Column<int>(type: "INT", nullable: true),
                    USERID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CONAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    REPORTDATE = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    COATITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MONTHYEAR = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    VCHDATE = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    VCHTYPE = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    VCHNO = table.Column<int>(type: "INT", nullable: true),
                    VCHDESCRIPTION = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    VCHREFERENCE = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    AMOUNT = table.Column<decimal>(type: "MONEY", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GL_SEGMENTS",
                columns: table => new
                {
                    SEGMENTID = table.Column<short>(type: "SMALLINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SEGMENTTITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SEGMENTPARENT = table.Column<short>(type: "SMALLINT", nullable: true),
                    SEGMENTTYPE = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    PAGEID = table.Column<short>(type: "SMALLINT", nullable: true),
                    ITEMROLE = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_SEGMENTS_PK", x => x.SEGMENTID);
                });

            migrationBuilder.CreateTable(
                name: "GL_TRIAL_BALANCE",
                columns: table => new
                {
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    COATITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    COALEVEL = table.Column<byte>(type: "TINYINT", nullable: true),
                    OPENDR = table.Column<decimal>(type: "MONEY", nullable: true),
                    OPENCR = table.Column<decimal>(type: "MONEY", nullable: true),
                    ACTIVITYDR = table.Column<decimal>(type: "MONEY", nullable: true),
                    ACTIVITYCR = table.Column<decimal>(type: "MONEY", nullable: true),
                    CLOSINGDR = table.Column<decimal>(type: "MONEY", nullable: true),
                    CLOSINGCR = table.Column<decimal>(type: "MONEY", nullable: true),
                    CONAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TBDATE = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    FROMACCOUNT = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    TOACCOUNT = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    CCCODE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    CCTITLE = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    REPORTLEVEL = table.Column<byte>(type: "TINYINT", nullable: true),
                    USERID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    GRAND_TOTAL = table.Column<bool>(type: "BIT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GL_VOUCHER",
                columns: table => new
                {
                    VCHCODE = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VCHTYPE = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    VCHTITLE = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    VCHNATURE = table.Column<byte>(type: "TINYINT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_VOUCHER_PK", x => x.VCHCODE);
                });

            migrationBuilder.CreateTable(
                name: "GL_COA",
                columns: table => new
                {
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    COATITLE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    COALEVEL = table.Column<byte>(type: "TINYINT", nullable: true),
                    COANATURE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    COATYPE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    CCCODE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_COA_PK", x => new { x.COCODE, x.COACODE });
                    table.ForeignKey(
                        name: "FK_COA",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                });

            migrationBuilder.CreateTable(
                name: "GL_COST_CENTER",
                columns: table => new
                {
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    CCCODE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    CCTITLE = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    CCLEVEL = table.Column<byte>(type: "TINYINT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_COST_CENTER_PK", x => new { x.COCODE, x.CCCODE });
                    table.ForeignKey(
                        name: "FK_COST_CENTER",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                });

            migrationBuilder.CreateTable(
                name: "GL_FISCAL_YEAR",
                columns: table => new
                {
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    COYEAR = table.Column<short>(type: "SMALLINT", nullable: false),
                    COMONTHID = table.Column<byte>(type: "TINYINT", nullable: false),
                    COMONTHNAME = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    PFROM = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    PTO = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    INITIAL_YEAR = table.Column<bool>(type: "BIT", nullable: true),
                    YEAR_CLOSED = table.Column<bool>(type: "BIT", nullable: true),
                    MONTH_CLOSED = table.Column<bool>(type: "BIT", nullable: true),
                    TYE_EXECUTED = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_FISCAL_YEAR_PK", x => new { x.COCODE, x.COYEAR, x.COMONTHID });
                    table.ForeignKey(
                        name: "FK_FISCAL_YEAR",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                });

            migrationBuilder.CreateTable(
                name: "GL_USERS",
                columns: table => new
                {
                    USERID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    COCODE = table.Column<int>(type: "INT", nullable: true),
                    COYEAR = table.Column<short>(type: "SMALLINT", nullable: true),
                    COMONTHID = table.Column<byte>(type: "TINYINT", nullable: true),
                    GROUPID = table.Column<short>(type: "SMALLINT", nullable: true),
                    PASSWORD = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ADMIN = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GL_USERS_PK", x => x.USERID);
                    table.ForeignKey(
                        name: "FK_USERS",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                    table.ForeignKey(
                        name: "FK_USERS2",
                        column: x => x.GROUPID,
                        principalTable: "GL_GROUPS_MASTER",
                        principalColumn: "GROUPID");
                });

            migrationBuilder.CreateTable(
                name: "GL_GROUPS_DETAIL",
                columns: table => new
                {
                    GROUPID = table.Column<short>(type: "SMALLINT", nullable: true),
                    SEGMENTID = table.Column<short>(type: "SMALLINT", nullable: true),
                    SEGMENTPARENT = table.Column<short>(type: "SMALLINT", nullable: true),
                    SEGMENTTYPE = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    PAGEID = table.Column<short>(type: "SMALLINT", nullable: true),
                    ITEMROLE = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ALLOW_ACCESS = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_GROUP_DETAIL",
                        column: x => x.GROUPID,
                        principalTable: "GL_GROUPS_MASTER",
                        principalColumn: "GROUPID");
                    table.ForeignKey(
                        name: "FK_USER_GROUPS",
                        column: x => x.SEGMENTID,
                        principalTable: "GL_SEGMENTS",
                        principalColumn: "SEGMENTID");
                });

            migrationBuilder.CreateTable(
                name: "GL_BANKS_OS",
                columns: table => new
                {
                    SR_NO = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    REMARKS = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    VCHDR = table.Column<decimal>(type: "MONEY", nullable: false),
                    VCHCR = table.Column<decimal>(type: "MONEY", nullable: false),
                    RECONCILED = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANKS_OS", x => x.SR_NO);
                    table.ForeignKey(
                        name: "FK_BANKS_OS1",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                    table.ForeignKey(
                        name: "FK_BANKS_OS2",
                        columns: x => new { x.COCODE, x.COACODE },
                        principalTable: "GL_COA",
                        principalColumns: new[] { "COCODE", "COACODE" });
                });

            migrationBuilder.CreateTable(
                name: "GL_BUDGET",
                columns: table => new
                {
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    COYEAR = table.Column<short>(type: "SMALLINT", nullable: true),
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    COANATURE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    CCCODE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    BUDGET_AMOUNT1 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT2 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT3 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT4 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT5 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT6 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT7 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT8 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT9 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT10 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT11 = table.Column<decimal>(type: "MONEY", nullable: true),
                    BUDGET_AMOUNT12 = table.Column<decimal>(type: "MONEY", nullable: true),
                    CRITERION = table.Column<bool>(type: "BIT", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_BUDGET1",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                    table.ForeignKey(
                        name: "FK_BUDGET2",
                        columns: x => new { x.COCODE, x.COACODE },
                        principalTable: "GL_COA",
                        principalColumns: new[] { "COCODE", "COACODE" });
                });

            migrationBuilder.CreateTable(
                name: "GL_TRAN_MASTER",
                columns: table => new
                {
                    TRAN_NO = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    COYEAR = table.Column<short>(type: "SMALLINT", nullable: false),
                    COMONTHID = table.Column<byte>(type: "TINYINT", nullable: false),
                    VCHCODE = table.Column<int>(type: "INT", nullable: false),
                    VCHNO = table.Column<int>(type: "INT", nullable: false),
                    VCHDATE = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    VCHDESCRIPTION = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    CREATEDBY = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    CREATEDON = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    VCHVERIFIED = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    VCHPOSTED = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    CLOSING = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAN_MASTER", x => x.TRAN_NO);
                    table.ForeignKey(
                        name: "FK_TRAN_MASTER1",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                    table.ForeignKey(
                        name: "FK_TRAN_MASTER2",
                        column: x => x.VCHCODE,
                        principalTable: "GL_VOUCHER",
                        principalColumn: "VCHCODE");
                    table.ForeignKey(
                        name: "FK_TRAN_MASTER3",
                        columns: x => new { x.COCODE, x.COYEAR, x.COMONTHID },
                        principalTable: "GL_FISCAL_YEAR",
                        principalColumns: new[] { "COCODE", "COYEAR", "COMONTHID" });
                });

            migrationBuilder.CreateTable(
                name: "GL_TRAN_DETAIL",
                columns: table => new
                {
                    LINE_NO = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRAN_NO = table.Column<int>(type: "INT", nullable: false),
                    COCODE = table.Column<int>(type: "INT", nullable: false),
                    COACODE = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    CCCODE = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    VCHDESCRIPTION = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    VCHDR = table.Column<decimal>(type: "MONEY", nullable: false),
                    VCHCR = table.Column<decimal>(type: "MONEY", nullable: false),
                    VCHREFERENCE = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    RECONCILED = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAN_DETAIL", x => x.LINE_NO);
                    table.ForeignKey(
                        name: "FK_TRAN_DETAIL1",
                        column: x => x.COCODE,
                        principalTable: "GL_COMPANY",
                        principalColumn: "COCODE");
                    table.ForeignKey(
                        name: "FK_TRAN_DETAIL2",
                        column: x => x.TRAN_NO,
                        principalTable: "GL_TRAN_MASTER",
                        principalColumn: "TRAN_NO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TRAN_DETAIL3",
                        columns: x => new { x.COCODE, x.CCCODE },
                        principalTable: "GL_COST_CENTER",
                        principalColumns: new[] { "COCODE", "CCCODE" });
                    table.ForeignKey(
                        name: "FK_TRAN_DETAIL4",
                        columns: x => new { x.COCODE, x.COACODE },
                        principalTable: "GL_COA",
                        principalColumns: new[] { "COCODE", "COACODE" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_GL_BANKS_OS_COCODE_COACODE",
                table: "GL_BANKS_OS",
                columns: new[] { "COCODE", "COACODE" });

            migrationBuilder.CreateIndex(
                name: "IX_GL_BUDGET_COCODE_COACODE",
                table: "GL_BUDGET",
                columns: new[] { "COCODE", "COACODE" });

            migrationBuilder.CreateIndex(
                name: "IX_GL_GROUPS_DETAIL_GROUPID",
                table: "GL_GROUPS_DETAIL",
                column: "GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_GL_GROUPS_DETAIL_SEGMENTID",
                table: "GL_GROUPS_DETAIL",
                column: "SEGMENTID");

            migrationBuilder.CreateIndex(
                name: "IX_GL_TRAN_DETAIL_COCODE_CCCODE",
                table: "GL_TRAN_DETAIL",
                columns: new[] { "COCODE", "CCCODE" });

            migrationBuilder.CreateIndex(
                name: "IX_GL_TRAN_DETAIL_COCODE_COACODE",
                table: "GL_TRAN_DETAIL",
                columns: new[] { "COCODE", "COACODE" });

            migrationBuilder.CreateIndex(
                name: "IX_GL_TRAN_DETAIL_TRAN_NO",
                table: "GL_TRAN_DETAIL",
                column: "TRAN_NO");

            migrationBuilder.CreateIndex(
                name: "IX_GL_TRAN_MASTER_COCODE_COYEAR_COMONTHID",
                table: "GL_TRAN_MASTER",
                columns: new[] { "COCODE", "COYEAR", "COMONTHID" });

            migrationBuilder.CreateIndex(
                name: "IX_GL_TRAN_MASTER_VCHCODE",
                table: "GL_TRAN_MASTER",
                column: "VCHCODE");

            migrationBuilder.CreateIndex(
                name: "IX_GL_USERS_COCODE",
                table: "GL_USERS",
                column: "COCODE");

            migrationBuilder.CreateIndex(
                name: "IX_GL_USERS_GROUPID",
                table: "GL_USERS",
                column: "GROUPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GL_BANKS_OS");

            migrationBuilder.DropTable(
                name: "GL_BUDGET");

            migrationBuilder.DropTable(
                name: "GL_BUDGET_REPORT");

            migrationBuilder.DropTable(
                name: "GL_DASHBOARD");

            migrationBuilder.DropTable(
                name: "GL_FEEDBACK");

            migrationBuilder.DropTable(
                name: "GL_FS_REPORT");

            migrationBuilder.DropTable(
                name: "GL_FS_SETUP");

            migrationBuilder.DropTable(
                name: "GL_GROUPS_DETAIL");

            migrationBuilder.DropTable(
                name: "GL_RECONCILE_REPORT");

            migrationBuilder.DropTable(
                name: "GL_TRAN_DETAIL");

            migrationBuilder.DropTable(
                name: "GL_TRIAL_BALANCE");

            migrationBuilder.DropTable(
                name: "GL_USERS");

            migrationBuilder.DropTable(
                name: "GL_SEGMENTS");

            migrationBuilder.DropTable(
                name: "GL_TRAN_MASTER");

            migrationBuilder.DropTable(
                name: "GL_COST_CENTER");

            migrationBuilder.DropTable(
                name: "GL_COA");

            migrationBuilder.DropTable(
                name: "GL_GROUPS_MASTER");

            migrationBuilder.DropTable(
                name: "GL_VOUCHER");

            migrationBuilder.DropTable(
                name: "GL_FISCAL_YEAR");

            migrationBuilder.DropTable(
                name: "GL_COMPANY");
        }
    }
}
