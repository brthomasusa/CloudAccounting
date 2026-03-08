using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudAccounting.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReseedCloudAcctgTestDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
                CREATE OR ALTER  Proc [dbo].[usp_ReseedCloudAcctgTestDb]
                AS
                BEGIN
                    BEGIN TRANSACTION;
                        BEGIN TRY
							DBCC CHECKIDENT ('dbo.GL_COMPANY', RESEED, 0);                    
							DBCC CHECKIDENT ('dbo.GL_VOUCHER', RESEED, 0);                    
                       
							-- GL_COMPANY
						   INSERT INTO dbo.GL_COMPANY 
								(CONAME, COADDRESS, COCITY, COZIP, COPHONE, COFAX, COCURRENCY) VALUES
								( 'BTechnical Consulting','123 Main Street','Dallas','75214','(972) 555-6666','(972) 555-5558','USD' );
						   INSERT INTO dbo.GL_COMPANY 
								(CONAME, COADDRESS, COCITY, COZIP, COPHONE, COFAX, COCURRENCY) VALUES
								( 'Contoso Ltd.','21477 Munger Blvd','Dallas','75214','(972) 555-6666','(972) 555-5558','USD' );
							INSERT INTO dbo.GL_COMPANY 
								(CONAME, COADDRESS, COCITY, COZIP, COPHONE, COFAX, COCURRENCY) VALUES
								( 'Fabrikam Inc.','123 Main Street','Dosoto','75214','(972) 555-6666','(972) 555-5558','USD' );
						   INSERT INTO dbo.GL_COMPANY 
								(CONAME, COADDRESS, COCITY, COZIP, COPHONE, COFAX, COCURRENCY) VALUES
								( 'Adventure Works','21477 Islamic Blvd','Dallas','75214','(972) 555-6666','(972) 555-5558','USD' );                        
                        
							-- GL_FISCAL_YEAR
							insert into gl_fiscal_year values (1,2024,1,'July','2024-07-01','2024-07-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,2,'August','2024-08-01','2024-08-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,3,'September','2024-09-01','2024-09-30',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,4,'October','2024-10-01','2024-10-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,5,'November','2024-11-01','2024-11-30',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,6,'December','2024-12-01','2024-12-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,7,'January','2025-01-01','2025-01-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,8,'February','2025-02-01','2025-02-28',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,9,'March','2025-03-01','2025-03-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,10,'April','2025-04-01','2025-04-30',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,11,'May','2025-05-01','2025-05-31',1,0,0,null);
							insert into gl_fiscal_year values (1,2024,12,'June','2025-06-01','2025-06-30',1,0,0,null);
							
							insert into gl_fiscal_year values (1,2025,1,'July','2025-07-01','2025-07-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,2,'August','2025-08-01','2025-08-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,3,'September','2025-09-01','2025-09-30',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,4,'October','2025-10-01','2025-10-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,5,'November','2025-11-01','2025-11-30',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,6,'December','2025-12-01','2025-12-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,7,'January','2026-01-01','2026-01-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,8,'February','2026-02-01','2026-02-28',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,9,'March','2026-03-01','2026-03-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,10,'April','2026-04-01','2026-04-30',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,11,'May','2026-05-01','2026-05-31',0,0,0,null);
							insert into gl_fiscal_year values (1,2025,12,'June','2026-06-01','2026-06-30',0,0,0,null);                         

							-- GL_VOUCHER
							INSERT INTO dbo.GL_VOUCHER (VCHTYPE, VCHTITLE, VCHNATURE) VALUES ('BPV', 'Bank Payment Voucher', 1);
							INSERT INTO dbo.GL_VOUCHER (VCHTYPE, VCHTITLE, VCHNATURE) VALUES ('LSI', 'Local Sales Invoice', 3);
							INSERT INTO dbo.GL_VOUCHER (VCHTYPE, VCHTITLE, VCHNATURE) VALUES ('BRV', 'Bank Receipt Voucher', 2);
							INSERT INTO dbo.GL_VOUCHER (VCHTYPE, VCHTITLE, VCHNATURE) VALUES ('ADJ', 'Adjustment Voucher', 3);
							INSERT INTO dbo.GL_VOUCHER (VCHTYPE, VCHTITLE, VCHNATURE) VALUES ('PO', 'Purchase Order', 1);                         

							COMMIT TRANSACTION;
                        END TRY
						BEGIN CATCH
                            ROLLBACK TRANSACTION

                            SELECT
                                ERROR_NUMBER() AS ErrorNumber,
                                ERROR_LINE() AS ErrorLine,
                                ERROR_MESSAGE() AS ErrorMessage;                
						END CATCH                               
                                       
                END";
            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[usp_ReseedCloudAcctgTestDb]";
            migrationBuilder.Sql(sp);
        }
    }
}
