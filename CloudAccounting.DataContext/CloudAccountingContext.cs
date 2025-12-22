using Microsoft.EntityFrameworkCore;
using CloudAccounting.EntityModels.Entities;
using System.Reflection;

namespace CloudAccounting.DataContext;

public partial class CloudAccountingContext : DbContext
{
    public CloudAccountingContext()
    {
    }

    public CloudAccountingContext(DbContextOptions<CloudAccountingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankOpeningStatement> BankOpeningStatements { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<BudgetReport> BudgetReports { get; set; }

    public virtual DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CostCenter> CostCenters { get; set; }

    public virtual DbSet<Dashboard> Dashboards { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<FiscalYear> FiscalYears { get; set; }

    public virtual DbSet<FinancialStatementReport> FinancialStatementReports { get; set; }

    public virtual DbSet<FinancialStatementSetup> FinancialStatementSetups { get; set; }

    public virtual DbSet<GroupsDetail> GroupsDetails { get; set; }

    public virtual DbSet<GroupsMaster> GroupsMasters { get; set; }

    public virtual DbSet<ReconcileReport> ReconcileReports { get; set; }

    public virtual DbSet<Segment> Segments { get; set; }

    public virtual DbSet<TransactionDetail> TranDetails { get; set; }

    public virtual DbSet<TransactionMaster> TranMasters { get; set; }

    public virtual DbSet<TrialBalance> TrialBalances { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOracle("User Id=CLOUD_ACCTG_DEV;Password=Info33Gum;Data Source=rhel9-ws:1521/ORCL;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseCollation("USING_NLS_COMP");

        modelBuilder.HasSequence("GL_BANKS_OS_SEQ");
        modelBuilder.HasSequence("GL_COMPANY_SEQ");
        modelBuilder.HasSequence("GL_FEEDBACK_SEQ");
        modelBuilder.HasSequence("GL_SEGMENTS_SEQ");
        modelBuilder.HasSequence("GL_TRAN_DETAIL_SEQ");
        modelBuilder.HasSequence("GL_TRAN_MASTER_SEQ");
        modelBuilder.HasSequence("GL_VOUCHER_SEQ");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
