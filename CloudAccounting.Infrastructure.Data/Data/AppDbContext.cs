using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CloudAccounting.Infrastructure.Data.Models;
using System.Reflection;

namespace CloudAccounting.Infrastructure.Data.Data
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<BankOpeningStatementDM> BankOpeningStatements { get; set; }

        public virtual DbSet<BudgetDM> Budgets { get; set; }

        public virtual DbSet<BudgetReportDM> BudgetReports { get; set; }

        public virtual DbSet<ChartOfAccountsDM> ChartOfAccounts { get; set; }

        public virtual DbSet<CompanyDM> Companies { get; set; }

        public virtual DbSet<CostCenterDM> CostCenters { get; set; }

        public virtual DbSet<DashboardDM> Dashboards { get; set; }

        public virtual DbSet<FeedbackDM> Feedbacks { get; set; }

        public virtual DbSet<FiscalYearDM> FiscalYears { get; set; }

        public virtual DbSet<FinancialStatementReportDM> FinancialStatementReports { get; set; }

        public virtual DbSet<FinancialStatementSetupDM> FinancialStatementSetups { get; set; }

        public virtual DbSet<GroupsDetailDM> GroupsDetails { get; set; }

        public virtual DbSet<GroupsMasterDM> GroupsMasters { get; set; }

        public virtual DbSet<ReconcileReportDM> ReconcileReports { get; set; }

        public virtual DbSet<SegmentDM> Segments { get; set; }

        public virtual DbSet<TransactionDetailDM> TranDetails { get; set; }

        public virtual DbSet<TransactionMasterDM> TranMasters { get; set; }

        public virtual DbSet<TrialBalanceDM> TrialBalances { get; set; }

        public virtual DbSet<UserDM> UserModels { get; set; }

        public virtual DbSet<VoucherDM> Vouchers { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // "Server=tcp:rhel9-ws,1433;Database=CloudAcctgTest;User Id=sa;Password=Info99Gum;TrustServerCertificate=True"
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}