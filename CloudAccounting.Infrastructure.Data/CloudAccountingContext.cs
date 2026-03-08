#pragma warning disable CS8604

using CloudAccounting.Infrastructure.Data.Models;
using System.Reflection;

namespace CloudAccounting.Infrastructure.Data;

public partial class CloudAccountingContext : DbContext
{
    public CloudAccountingContext()
    {
    }

    public CloudAccountingContext(DbContextOptions<CloudAccountingContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

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

    public virtual DbSet<UserDM> Users { get; set; }

    public virtual DbSet<VoucherDM> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=tcp:rhel9-ws,1433;Database=CloudAcctgTest;User Id=sa;Password=Info99Gum;TrustServerCertificate=True");

    // "Server=tcp:rhel9-ws,1433;Database=CloudAcctgTest;User Id=sa;Password=Info99Gum;TrustServerCertificate=True"
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
