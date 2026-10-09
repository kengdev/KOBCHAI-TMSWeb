using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TMSWeb.Models.Entities.LOG;

public partial class DbTMSContext : DbContext
{
    public DbTMSContext(DbContextOptions<DbTMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdvanceExpenseType> AdvanceExpenseTypes { get; set; }

    public virtual DbSet<AdvanceFund> AdvanceFunds { get; set; }

    public virtual DbSet<AdvanceFundTransaction> AdvanceFundTransactions { get; set; }

    public virtual DbSet<AdvanceRequest> AdvanceRequests { get; set; }

    public virtual DbSet<AdvanceRequestReference> AdvanceRequestReferences { get; set; }

    public virtual DbSet<LcProvince> LcProvinces { get; set; }

    public virtual DbSet<LogSystem> LogSystems { get; set; }

    public virtual DbSet<SeGroup> SeGroups { get; set; }

    public virtual DbSet<SeGroupPriviledge01> SeGroupPriviledge01s { get; set; }

    public virtual DbSet<SeGroupPriviledge02> SeGroupPriviledge02s { get; set; }

    public virtual DbSet<SeGroupPriviledge03> SeGroupPriviledge03s { get; set; }

    public virtual DbSet<SeLevel01> SeLevel01s { get; set; }

    public virtual DbSet<SeLevel02> SeLevel02s { get; set; }

    public virtual DbSet<SeLevel03> SeLevel03s { get; set; }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblAgent> TblAgents { get; set; }

    public virtual DbSet<TblAllowance> TblAllowances { get; set; }

    public virtual DbSet<TblAllowanceDetail> TblAllowanceDetails { get; set; }

    public virtual DbSet<TblBank> TblBanks { get; set; }

    public virtual DbSet<TblBindingGenerate> TblBindingGenerates { get; set; }

    public virtual DbSet<TblBrand> TblBrands { get; set; }

    public virtual DbSet<TblCodeGenerate> TblCodeGenerates { get; set; }

    public virtual DbSet<TblContainerSize> TblContainerSizes { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblCustomerAddress> TblCustomerAddresses { get; set; }

    public virtual DbSet<TblCustomerCost> TblCustomerCosts { get; set; }

    public virtual DbSet<TblCustomerRule> TblCustomerRules { get; set; }

    public virtual DbSet<TblCustomerType> TblCustomerTypes { get; set; }

    public virtual DbSet<TblDriver> TblDrivers { get; set; }

    public virtual DbSet<TblDriverCost> TblDriverCosts { get; set; }

    public virtual DbSet<TblDriverRate> TblDriverRates { get; set; }

    public virtual DbSet<TblDriverSpecialvalue> TblDriverSpecialvalues { get; set; }

    public virtual DbSet<TblFleetRate> TblFleetRates { get; set; }

    public virtual DbSet<TblFleetSupplier> TblFleetSuppliers { get; set; }

    public virtual DbSet<TblFleetSupplierType> TblFleetSupplierTypes { get; set; }

    public virtual DbSet<TblFleetpayment> TblFleetpayments { get; set; }

    public virtual DbSet<TblFleetpaymentDetail> TblFleetpaymentDetails { get; set; }

    public virtual DbSet<TblFuelRate> TblFuelRates { get; set; }

    public virtual DbSet<TblFuelType> TblFuelTypes { get; set; }

    public virtual DbSet<TblGroup> TblGroups { get; set; }

    public virtual DbSet<TblIndustry> TblIndustries { get; set; }

    public virtual DbSet<TblInsurance> TblInsurances { get; set; }

    public virtual DbSet<TblInsuranceType> TblInsuranceTypes { get; set; }

    public virtual DbSet<TblJobAllowance> TblJobAllowances { get; set; }

    public virtual DbSet<TblJobDetail> TblJobDetails { get; set; }

    public virtual DbSet<TblJobImg> TblJobImgs { get; set; }

    public virtual DbSet<TblJobMaster> TblJobMasters { get; set; }

    public virtual DbSet<TblJobOcean> TblJobOceans { get; set; }

    public virtual DbSet<TblJobPlace> TblJobPlaces { get; set; }

    public virtual DbSet<TblJobTracking> TblJobTrackings { get; set; }

    public virtual DbSet<TblJobTruck> TblJobTrucks { get; set; }

    public virtual DbSet<TblJobType> TblJobTypes { get; set; }

    public virtual DbSet<TblJobadvance> TblJobadvances { get; set; }

    public virtual DbSet<TblJobadvanceOther> TblJobadvanceOthers { get; set; }

    public virtual DbSet<TblJobadvancePayment> TblJobadvancePayments { get; set; }

    public virtual DbSet<TblJobcashflow> TblJobcashflows { get; set; }

    public virtual DbSet<TblJobcashflowAdvance> TblJobcashflowAdvances { get; set; }

    public virtual DbSet<TblJobcashflowAdvanceOther> TblJobcashflowAdvanceOthers { get; set; }

    public virtual DbSet<TblJobcashflowCashing> TblJobcashflowCashings { get; set; }

    public virtual DbSet<TblJobcashing> TblJobcashings { get; set; }

    public virtual DbSet<TblPlace> TblPlaces { get; set; }

    public virtual DbSet<TblPortCashing> TblPortCashings { get; set; }

    public virtual DbSet<TblReceipt> TblReceipts { get; set; }

    public virtual DbSet<TblReceiptType> TblReceiptTypes { get; set; }

    public virtual DbSet<TblRequest> TblRequests { get; set; }

    public virtual DbSet<TblShipper> TblShippers { get; set; }

    public virtual DbSet<TblSupplier> TblSuppliers { get; set; }

    public virtual DbSet<TblSupplierAllowance> TblSupplierAllowances { get; set; }

    public virtual DbSet<TblSupplierRate> TblSupplierRates { get; set; }

    public virtual DbSet<TblSupplierType> TblSupplierTypes { get; set; }

    public virtual DbSet<TblSupplierpayment> TblSupplierpayments { get; set; }

    public virtual DbSet<TblSupplierpaymentDetail> TblSupplierpaymentDetails { get; set; }

    public virtual DbSet<TblTrailer> TblTrailers { get; set; }

    public virtual DbSet<TblTrailerFloor> TblTrailerFloors { get; set; }

    public virtual DbSet<TblTrailerInsurance> TblTrailerInsurances { get; set; }

    public virtual DbSet<TblTrailerType> TblTrailerTypes { get; set; }

    public virtual DbSet<TblTruck> TblTrucks { get; set; }

    public virtual DbSet<TblTruckDetail> TblTruckDetails { get; set; }

    public virtual DbSet<TblTruckDriver> TblTruckDrivers { get; set; }

    public virtual DbSet<TblTruckInsurance> TblTruckInsurances { get; set; }

    public virtual DbSet<TblTruckTrailer> TblTruckTrailers { get; set; }

    public virtual DbSet<TblTruckType> TblTruckTypes { get; set; }

    public virtual DbSet<TblYardCashing> TblYardCashings { get; set; }

    public virtual DbSet<TblYardport> TblYardports { get; set; }

    public virtual DbSet<TblZone> TblZones { get; set; }

    public virtual DbSet<ViewBillingListActive> ViewBillingListActives { get; set; }

    public virtual DbSet<ViewBillingOtherListActive> ViewBillingOtherListActives { get; set; }

    public virtual DbSet<ViewCostListActive> ViewCostListActives { get; set; }

    public virtual DbSet<ViewCoverSheetListActive> ViewCoverSheetListActives { get; set; }

    public virtual DbSet<ViewDriverListActive> ViewDriverListActives { get; set; }

    public virtual DbSet<ViewDriverPaymentListActive> ViewDriverPaymentListActives { get; set; }

    public virtual DbSet<ViewEmployeeListActive> ViewEmployeeListActives { get; set; }

    public virtual DbSet<ViewGeneralBudgetTransaction> ViewGeneralBudgetTransactions { get; set; }

    public virtual DbSet<ViewGroupPriviledgeLevel1> ViewGroupPriviledgeLevel1s { get; set; }

    public virtual DbSet<ViewGroupPriviledgeLevel2> ViewGroupPriviledgeLevel2s { get; set; }

    public virtual DbSet<ViewGroupPriviledgeLevel3> ViewGroupPriviledgeLevel3s { get; set; }

    public virtual DbSet<ViewJobAdvanceCashingListActive> ViewJobAdvanceCashingListActives { get; set; }

    public virtual DbSet<ViewJobCashingListActive> ViewJobCashingListActives { get; set; }

    public virtual DbSet<ViewJobListActive> ViewJobListActives { get; set; }

    public virtual DbSet<ViewJobSummary> ViewJobSummaries { get; set; }

    public virtual DbSet<ViewOrderListActive> ViewOrderListActives { get; set; }

    public virtual DbSet<ViewTrailerInsuranceList> ViewTrailerInsuranceLists { get; set; }

    public virtual DbSet<ViewTrailerListActive> ViewTrailerListActives { get; set; }

    public virtual DbSet<ViewTransportBudgetTransaction> ViewTransportBudgetTransactions { get; set; }

    public virtual DbSet<ViewTruckInsuranceListActive> ViewTruckInsuranceListActives { get; set; }

    public virtual DbSet<ViewTruckListActive> ViewTruckListActives { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdvanceExpenseType>(entity =>
        {
            entity.HasKey(e => e.ExpenseTypeId);

            entity.ToTable("advance_expense_type");

            entity.HasIndex(e => e.ExpenseTypeCode, "UQ_advance_expense_type_code").IsUnique();

            entity.Property(e => e.ExpenseTypeId).HasColumnName("expense_type_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_expense_type_created_date")
                .HasColumnName("created_date");
            entity.Property(e => e.ExpenseTypeCode)
                .HasMaxLength(30)
                .HasColumnName("expense_type_code");
            entity.Property(e => e.ExpenseTypeName)
                .HasMaxLength(200)
                .HasColumnName("expense_type_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true, "DF_advance_expense_type_is_active")
                .HasColumnName("is_active");
            entity.Property(e => e.IsTripRelated).HasColumnName("is_trip_related");
            entity.Property(e => e.RequireDocument).HasColumnName("require_document");
            entity.Property(e => e.RequireSettlement)
                .HasDefaultValue(true, "DF_advance_expense_type_require_settlement")
                .HasColumnName("require_settlement");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        });

        modelBuilder.Entity<AdvanceFund>(entity =>
        {
            entity.HasKey(e => e.FundId);

            entity.ToTable("advance_fund");

            entity.HasIndex(e => e.FundCode, "UQ_advance_fund_fund_code").IsUnique();

            entity.HasIndex(e => e.FundGuid, "UQ_advance_fund_fund_guid").IsUnique();

            entity.Property(e => e.FundId).HasColumnName("fund_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_fund_created_date")
                .HasColumnName("created_date");
            entity.Property(e => e.CurrentBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("current_balance");
            entity.Property(e => e.FundCode)
                .HasMaxLength(20)
                .HasColumnName("fund_code");
            entity.Property(e => e.FundGuid)
                .HasDefaultValueSql("(newid())", "DF_advance_fund_fund_guid")
                .HasColumnName("fund_guid");
            entity.Property(e => e.FundName)
                .HasMaxLength(200)
                .HasColumnName("fund_name");
            entity.Property(e => e.HolderCode)
                .HasMaxLength(50)
                .HasColumnName("holder_code");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true, "DF_advance_fund_is_active")
                .HasColumnName("is_active");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasColumnName("remark");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
        });

        modelBuilder.Entity<AdvanceFundTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("advance_fund_transaction");

            entity.HasIndex(e => new { e.FundId, e.TransactionDate }, "IX_advance_fund_transaction_fund_date");

            entity.HasIndex(e => e.ReferenceTransactionId, "IX_advance_fund_transaction_reference").HasFilter("([reference_transaction_id] IS NOT NULL)");

            entity.HasIndex(e => e.RequestId, "IX_advance_fund_transaction_request").HasFilter("([request_id] IS NOT NULL)");

            entity.HasIndex(e => e.TransactionGuid, "UQ_advance_fund_transaction_guid").IsUnique();

            entity.HasIndex(e => e.RequestId, "UX_advance_fund_transaction_request_payment")
                .IsUnique()
                .HasFilter("([request_id] IS NOT NULL AND [transaction_type]='PAYMENT')");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CashflowId).HasColumnName("cashflow_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_fund_transaction_created_date")
                .HasColumnName("created_date");
            entity.Property(e => e.FundId).HasColumnName("fund_id");
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(100)
                .HasColumnName("reference_no");
            entity.Property(e => e.ReferenceTransactionId).HasColumnName("reference_transaction_id");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasColumnName("remark");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_fund_transaction_date")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionGuid)
                .HasDefaultValueSql("(newid())", "DF_advance_fund_transaction_guid")
                .HasColumnName("transaction_guid");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.Fund).WithMany(p => p.AdvanceFundTransactions)
                .HasForeignKey(d => d.FundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_advance_fund_transaction_fund");

            entity.HasOne(d => d.ReferenceTransaction).WithMany(p => p.InverseReferenceTransaction)
                .HasForeignKey(d => d.ReferenceTransactionId)
                .HasConstraintName("FK_advance_fund_transaction_reference");

            entity.HasOne(d => d.Request).WithOne(p => p.AdvanceFundTransaction)
                .HasForeignKey<AdvanceFundTransaction>(d => d.RequestId)
                .HasConstraintName("FK_advance_fund_transaction_request");
        });

        modelBuilder.Entity<AdvanceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("advance_request");

            entity.HasIndex(e => e.JobDetailCode, "IX_advance_request_job_detail_code").HasFilter("([job_detail_code] IS NOT NULL)");

            entity.HasIndex(e => e.RequestCode, "UQ_advance_request_code").IsUnique();

            entity.HasIndex(e => e.RequestGuid, "UQ_advance_request_guid").IsUnique();

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ApprovedAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("approved_amount");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .HasColumnName("approved_by");
            entity.Property(e => e.ApprovedDate).HasColumnName("approved_date");
            entity.Property(e => e.CancelledBy)
                .HasMaxLength(50)
                .HasColumnName("cancelled_by");
            entity.Property(e => e.CancelledDate).HasColumnName("cancelled_date");
            entity.Property(e => e.CancelledReason)
                .HasMaxLength(500)
                .HasColumnName("cancelled_reason");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_request_created_date")
                .HasColumnName("created_date");
            entity.Property(e => e.ExpenseTypeId).HasColumnName("expense_type_id");
            entity.Property(e => e.FundId).HasColumnName("fund_id");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.PaidBy)
                .HasMaxLength(50)
                .HasColumnName("paid_by");
            entity.Property(e => e.PaidDate).HasColumnName("paid_date");
            entity.Property(e => e.PayeeCode)
                .HasMaxLength(50)
                .HasColumnName("payee_code");
            entity.Property(e => e.PayeeName)
                .HasMaxLength(200)
                .HasColumnName("payee_name");
            entity.Property(e => e.PayeeType)
                .HasMaxLength(30)
                .HasColumnName("payee_type");
            entity.Property(e => e.Purpose)
                .HasMaxLength(500)
                .HasColumnName("purpose");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasColumnName("remark");
            entity.Property(e => e.RequestAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("request_amount");
            entity.Property(e => e.RequestCode)
                .HasMaxLength(30)
                .HasColumnName("request_code");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_request_date")
                .HasColumnName("request_date");
            entity.Property(e => e.RequestGuid)
                .HasDefaultValueSql("(newid())", "DF_advance_request_guid")
                .HasColumnName("request_guid");
            entity.Property(e => e.RequestedBy)
                .HasMaxLength(50)
                .HasColumnName("requested_by");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("DRAFT", "DF_advance_request_status")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(d => d.ExpenseType).WithMany(p => p.AdvanceRequests)
                .HasForeignKey(d => d.ExpenseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_advance_request_expense_type");

            entity.HasOne(d => d.Fund).WithMany(p => p.AdvanceRequests)
                .HasForeignKey(d => d.FundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_advance_request_fund");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.AdvanceRequests)
                .HasForeignKey(d => d.JobDetailCode)
                .HasConstraintName("FK_advance_request_tbl_job_detail");
        });

        modelBuilder.Entity<AdvanceRequestReference>(entity =>
        {
            entity.HasKey(e => e.ReferenceId);

            entity.ToTable("advance_request_reference");

            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_advance_request_reference_created_date")
                .HasColumnName("created_date");
            entity.Property(e => e.IsPrimary).HasColumnName("is_primary");
            entity.Property(e => e.ReferenceCode)
                .HasMaxLength(100)
                .HasColumnName("reference_code");
            entity.Property(e => e.ReferenceName)
                .HasMaxLength(300)
                .HasColumnName("reference_name");
            entity.Property(e => e.ReferenceType)
                .HasMaxLength(30)
                .HasColumnName("reference_type");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasColumnName("remark");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Request).WithMany(p => p.AdvanceRequestReferences)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_advance_request_reference_request");
        });

        modelBuilder.Entity<LcProvince>(entity =>
        {
            entity.HasKey(e => e.ProvinceCode).HasName("PK_lc_province_1");

            entity.ToTable("lc_province");

            entity.Property(e => e.ProvinceCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("province_code");
            entity.Property(e => e.ProvinceNameEng)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("province_name_eng");
            entity.Property(e => e.ProvinceNameTha)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("province_name_tha");
        });

        modelBuilder.Entity<LogSystem>(entity =>
        {
            entity.ToTable("log_system");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.LogAction)
                .HasColumnType("text")
                .HasColumnName("log_action");
            entity.Property(e => e.LogReferenceCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("log_reference_code");
        });

        modelBuilder.Entity<SeGroup>(entity =>
        {
            entity.HasKey(e => e.GroupCode);

            entity.ToTable("se_group");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("group_name");
        });

        modelBuilder.Entity<SeGroupPriviledge01>(entity =>
        {
            entity.HasKey(e => new { e.GroupCode, e.Level01Code });

            entity.ToTable("se_group_priviledge_01");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.Level01Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_01_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.GroupCodeNavigation).WithMany(p => p.SeGroupPriviledge01s)
                .HasForeignKey(d => d.GroupCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_group_priviledge_01_tbl_group");

            entity.HasOne(d => d.Level01CodeNavigation).WithMany(p => p.SeGroupPriviledge01s)
                .HasForeignKey(d => d.Level01Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_group_priviledge_01_se_level_01");
        });

        modelBuilder.Entity<SeGroupPriviledge02>(entity =>
        {
            entity.HasKey(e => new { e.GroupCode, e.Level02Code });

            entity.ToTable("se_group_priviledge_02");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.Level02Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_02_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.GroupCodeNavigation).WithMany(p => p.SeGroupPriviledge02s)
                .HasForeignKey(d => d.GroupCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_group_priviledge_02_tbl_group");

            entity.HasOne(d => d.Level02CodeNavigation).WithMany(p => p.SeGroupPriviledge02s)
                .HasForeignKey(d => d.Level02Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_group_priviledge_02_se_level_02");
        });

        modelBuilder.Entity<SeGroupPriviledge03>(entity =>
        {
            entity.HasKey(e => new { e.GroupCode, e.Level03Code }).HasName("PK_se_group_priviledge");

            entity.ToTable("se_group_priviledge_03");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.Level03Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_03_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.GroupCodeNavigation).WithMany(p => p.SeGroupPriviledge03s)
                .HasForeignKey(d => d.GroupCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_group_priviledge_tbl_group");

            entity.HasOne(d => d.Level03CodeNavigation).WithMany(p => p.SeGroupPriviledge03s)
                .HasForeignKey(d => d.Level03Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_group_priviledge_se_level_03");
        });

        modelBuilder.Entity<SeLevel01>(entity =>
        {
            entity.HasKey(e => e.Level01Code).HasName("PK_tbl_level_01");

            entity.ToTable("se_level_01");

            entity.Property(e => e.Level01Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_01_code");
            entity.Property(e => e.Level01Active)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_01_active");
            entity.Property(e => e.Level01Icon)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_01_icon");
            entity.Property(e => e.Level01Index).HasColumnName("level_01_index");
            entity.Property(e => e.Level01Link)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("level_01_link");
            entity.Property(e => e.Level01Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_01_name");
        });

        modelBuilder.Entity<SeLevel02>(entity =>
        {
            entity.HasKey(e => e.Level02Code).HasName("PK_tbl_level_02");

            entity.ToTable("se_level_02");

            entity.Property(e => e.Level02Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_02_code");
            entity.Property(e => e.Level01Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_01_code");
            entity.Property(e => e.Level02Active)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_02_active");
            entity.Property(e => e.Level02Index)
                .HasDefaultValue(1, "DF_se_level_02_level_02_index")
                .HasColumnName("level_02_index");
            entity.Property(e => e.Level02Link)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("level_02_link");
            entity.Property(e => e.Level02Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_02_name");

            entity.HasOne(d => d.Level01CodeNavigation).WithMany(p => p.SeLevel02s)
                .HasForeignKey(d => d.Level01Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_level_02_tbl_level_01");
        });

        modelBuilder.Entity<SeLevel03>(entity =>
        {
            entity.HasKey(e => e.Level03Code);

            entity.ToTable("se_level_03");

            entity.Property(e => e.Level03Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_03_code");
            entity.Property(e => e.Level02Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_02_code");
            entity.Property(e => e.Level03Active)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_03_active");
            entity.Property(e => e.Level03Link)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("level_03_link");
            entity.Property(e => e.Level03Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_03_name");

            entity.HasOne(d => d.Level02CodeNavigation).WithMany(p => p.SeLevel03s)
                .HasForeignKey(d => d.Level02Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_se_level_03_se_level_02");
        });

        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity.HasKey(e => e.AccUsername);

            entity.ToTable("tbl_account");

            entity.Property(e => e.AccUsername)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("acc_username");
            entity.Property(e => e.AccFullname)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("acc_fullname");
            entity.Property(e => e.AccPassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("acc_password");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.LineId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("line_id");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.GroupCodeNavigation).WithMany(p => p.TblAccounts)
                .HasForeignKey(d => d.GroupCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_account_tbl_group");
        });

        modelBuilder.Entity<TblAgent>(entity =>
        {
            entity.HasKey(e => e.AgentCode);

            entity.ToTable("tbl_agent");

            entity.Property(e => e.AgentCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("agent_code");
            entity.Property(e => e.AgentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("agent_name");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblAllowance>(entity =>
        {
            entity.HasKey(e => e.AllowanceCode);

            entity.ToTable("tbl_allowance");

            entity.Property(e => e.AllowanceCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("allowance_code");
            entity.Property(e => e.AdditionalTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("additional_total");
            entity.Property(e => e.AllowanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("allowance_total");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ExchangeTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("exchange_total");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsPayment).HasColumnName("is_payment");
            entity.Property(e => e.Remark)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("remark");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.SubtractTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("subtract_total");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblAllowances)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_allowance_tbl_driver");
        });

        modelBuilder.Entity<TblAllowanceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_allowance_detail_1");

            entity.ToTable("tbl_allowance_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllowanceCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("allowance_code");
            entity.Property(e => e.AllowanceDate).HasColumnName("allowance_date");
            entity.Property(e => e.FuelValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fuel_value");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.OtherValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("other_value");
            entity.Property(e => e.TripType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trip_type");
            entity.Property(e => e.TripValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trip_value");
            entity.Property(e => e.WasteValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("waste_value");

            entity.HasOne(d => d.AllowanceCodeNavigation).WithMany(p => p.TblAllowanceDetails)
                .HasForeignKey(d => d.AllowanceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_allowance_detail_tbl_allowance");
        });

        modelBuilder.Entity<TblBank>(entity =>
        {
            entity.HasKey(e => e.BankCode);

            entity.ToTable("tbl_bank");

            entity.Property(e => e.BankCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("bank_code");
            entity.Property(e => e.BankName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("bank_name");
        });

        modelBuilder.Entity<TblBindingGenerate>(entity =>
        {
            entity.HasKey(e => new { e.JobCode, e.JobDetailCode });

            entity.ToTable("tbl_binding_generate");

            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IndexCode).HasColumnName("index_code");
            entity.Property(e => e.Is2short).HasColumnName("is_2short");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.JobCodeNavigation).WithMany(p => p.TblBindingGenerates)
                .HasForeignKey(d => d.JobCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_binding_generate_tbl_job_master");
        });

        modelBuilder.Entity<TblBrand>(entity =>
        {
            entity.HasKey(e => e.BrandCode);

            entity.ToTable("tbl_brand");

            entity.Property(e => e.BrandCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("brand_code");
            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("brand_name");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblCodeGenerate>(entity =>
        {
            entity.HasKey(e => e.CodeName);

            entity.ToTable("tbl_code_generate");

            entity.Property(e => e.CodeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code_name");
            entity.Property(e => e.CodeType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code_type");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<TblContainerSize>(entity =>
        {
            entity.HasKey(e => e.ContainerSizeCode).HasName("PK_tbl_container_type");

            entity.ToTable("tbl_container_size");

            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.ContainerLiftOff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("container_lift_off");
            entity.Property(e => e.ContainerSizeGroup)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_group");
            entity.Property(e => e.ContainerSizeName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_size_name");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerCode);

            entity.ToTable("tbl_customer");

            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_customer_customer_address")
                .HasColumnName("customer_address");
            entity.Property(e => e.CustomerExten)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customer_exten");
            entity.Property(e => e.CustomerNameEn)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_en");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.CustomerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_type_code");
            entity.Property(e => e.CustomerVatNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_customer_customer_vat_no")
                .HasColumnName("customer_vat_no");
            entity.Property(e => e.DocType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("doc_type");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.VendorCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_customer_vendor_code")
                .HasColumnName("vendor_code");

            entity.HasOne(d => d.CustomerTypeCodeNavigation).WithMany(p => p.TblCustomers)
                .HasForeignKey(d => d.CustomerTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_customer_tbl_customer_type");
        });

        modelBuilder.Entity<TblCustomerAddress>(entity =>
        {
            entity.HasKey(e => e.CustomerCode);

            entity.ToTable("tbl_customer_address");

            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.AdvanceAddress)
                .HasColumnType("text")
                .HasColumnName("advance_address");
            entity.Property(e => e.AdvanceName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("advance_name");
            entity.Property(e => e.TransportAddress)
                .HasColumnType("text")
                .HasColumnName("transport_address");
            entity.Property(e => e.TransportName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("transport_name");

            entity.HasOne(d => d.CustomerCodeNavigation).WithOne(p => p.TblCustomerAddress)
                .HasForeignKey<TblCustomerAddress>(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_customer_address_tbl_customer");
        });

        modelBuilder.Entity<TblCustomerCost>(entity =>
        {
            entity.HasKey(e => e.CostCode).HasName("PK_tbl_customer_cost_1");

            entity.ToTable("tbl_customer_cost");

            entity.HasIndex(e => e.CostCode, "IX_tbl_customer_cost").IsUnique();

            entity.Property(e => e.CostCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cost_code");
            entity.Property(e => e.CostRemark)
                .HasColumnType("text")
                .HasColumnName("cost_remark");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.JobName)
                .HasMaxLength(150)
                .HasColumnName("job_name");
            entity.Property(e => e.MissionCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mission_code");
            entity.Property(e => e.ReceiptAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_customer_cost_address1")
                .HasColumnName("receipt_address");
            entity.Property(e => e.ReceiptName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_customer_cost_receipt_name")
                .HasColumnName("receipt_name");
            entity.Property(e => e.ReceiptTax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_customer_cost_address2")
                .HasColumnName("receipt_tax");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.TblCustomerCosts)
                .HasForeignKey(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_customer_cost_tbl_customer");
        });

        modelBuilder.Entity<TblCustomerRule>(entity =>
        {
            entity.HasKey(e => e.CustomerCode).HasName("PK_tbl_customer_payment");

            entity.ToTable("tbl_customer_rule");

            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.IsAdvance).HasColumnName("is_advance");
            entity.Property(e => e.IsBypassTripcost).HasColumnName("is_bypass_tripcost");
            entity.Property(e => e.IsLiftCashing).HasColumnName("is_lift_cashing");
            entity.Property(e => e.IsLiftoff).HasColumnName("is_liftoff");
            entity.Property(e => e.IsLifton).HasColumnName("is_lifton");
            entity.Property(e => e.IsPort).HasColumnName("is_port");
            entity.Property(e => e.IsTransportOther)
                .HasDefaultValue(true, "DF_tbl_customer_rule_is_transport_other")
                .HasColumnName("is_transport_other");
            entity.Property(e => e.IsYard).HasColumnName("is_yard");

            entity.HasOne(d => d.CustomerCodeNavigation).WithOne(p => p.TblCustomerRule)
                .HasForeignKey<TblCustomerRule>(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_customer_payment_tbl_customer");
        });

        modelBuilder.Entity<TblCustomerType>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeCode);

            entity.ToTable("tbl_customer_type");

            entity.Property(e => e.CustomerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_type_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerTypeName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("customer_type_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblDriver>(entity =>
        {
            entity.HasKey(e => e.DriverCode).HasName("PK_tbl_driver_1");

            entity.ToTable("tbl_driver");

            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
        });

        modelBuilder.Entity<TblDriverCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_driver_cost_1");

            entity.ToTable("tbl_driver_cost");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.IsWeeklypayment).HasColumnName("is_weeklypayment");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.TripRound)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trip_round");
            entity.Property(e => e.TripValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trip_value");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblDriverCosts)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_driver_cost_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblDriverCosts)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_driver_cost_tbl_driver_cost");
        });

        modelBuilder.Entity<TblDriverRate>(entity =>
        {
            entity.HasKey(e => e.PlaceCode).HasName("PK_tbl_placerate");

            entity.ToTable("tbl_driver_rate");

            entity.Property(e => e.PlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("place_code");
            entity.Property(e => e.RateValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("rate_value");
        });

        modelBuilder.Entity<TblDriverSpecialvalue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_driver_subtract");

            entity.ToTable("tbl_driver_specialvalue");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllowanceCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("allowance_code");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.Specialvalue)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("specialvalue");
            entity.Property(e => e.SpecialvalueName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("specialvalue_name");
            entity.Property(e => e.SpecialvalueType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("specialvalue_type");

            entity.HasOne(d => d.AllowanceCodeNavigation).WithMany(p => p.TblDriverSpecialvalues)
                .HasForeignKey(d => d.AllowanceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_driver_subtract_tbl_allowance");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblDriverSpecialvalues)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_driver_subtract_tbl_driver");
        });

        modelBuilder.Entity<TblFleetRate>(entity =>
        {
            entity.HasKey(e => new { e.FleetSupplierTypeCode, e.PlaceCode }).HasName("PK_tbl_fleet_cost_rate");

            entity.ToTable("tbl_fleet_rate");

            entity.Property(e => e.FleetSupplierTypeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_type_code");
            entity.Property(e => e.PlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("place_code");
            entity.Property(e => e.Trailer10w)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trailer_10W");
            entity.Property(e => e.Trailer18w)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trailer_18W");
            entity.Property(e => e.Trailer6w)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trailer_6W");
            entity.Property(e => e.TrailerExtra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trailer_extra");
            entity.Property(e => e.Truck10w)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("truck_10W");
            entity.Property(e => e.Truck4w)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("truck_4W");
            entity.Property(e => e.Truck6w)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("truck_6W");

            entity.HasOne(d => d.FleetSupplierTypeCodeNavigation).WithMany(p => p.TblFleetRates)
                .HasForeignKey(d => d.FleetSupplierTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_fleet_rate_tbl_fleet_supplier_type");

            entity.HasOne(d => d.PlaceCodeNavigation).WithMany(p => p.TblFleetRates)
                .HasForeignKey(d => d.PlaceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_fleet_rate_tbl_place");
        });

        modelBuilder.Entity<TblFleetSupplier>(entity =>
        {
            entity.HasKey(e => e.FleetSupplierCode);

            entity.ToTable("tbl_fleet_supplier");

            entity.Property(e => e.FleetSupplierCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.FleetSupplierName)
                .HasMaxLength(250)
                .HasColumnName("fleet_supplier_name");
            entity.Property(e => e.FleetSuppplierTypeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_suppplier_type_code");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.Remark).HasColumnName("remark");

            entity.HasOne(d => d.FleetSuppplierTypeCodeNavigation).WithMany(p => p.TblFleetSuppliers)
                .HasForeignKey(d => d.FleetSuppplierTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_fleet_supplier_tbl_fleet_supplier_type");
        });

        modelBuilder.Entity<TblFleetSupplierType>(entity =>
        {
            entity.HasKey(e => e.FleetSupplierTypeCode);

            entity.ToTable("tbl_fleet_supplier_type");

            entity.Property(e => e.FleetSupplierTypeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_type_code");
            entity.Property(e => e.FleetSupplierTypeName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("fleet_supplier_type_name");
        });

        modelBuilder.Entity<TblFleetpayment>(entity =>
        {
            entity.HasKey(e => e.FleetpaymentCode);

            entity.ToTable("tbl_fleetpayment");

            entity.Property(e => e.FleetpaymentCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleetpayment_code");
            entity.Property(e => e.AreaTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("area_total");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ExpressTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("express_total");
            entity.Property(e => e.FleetpaymentAmount).HasColumnName("fleetpayment_amount");
            entity.Property(e => e.FleetpaymentMonth)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleetpayment_month");
            entity.Property(e => e.FleetpaymentNetTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fleetpayment_net_total");
            entity.Property(e => e.FleetpaymentRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("fleetpayment_remark");
            entity.Property(e => e.FleetpaymentTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fleetpayment_total");
            entity.Property(e => e.FleetpaymentVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fleetpayment_vat");
            entity.Property(e => e.FleetpaymentYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleetpayment_year");
            entity.Property(e => e.FuelLitreTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fuel_litre_total");
            entity.Property(e => e.FuelRate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fuel_rate");
            entity.Property(e => e.FuelTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fuel_total");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.MaintenanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("maintenance_total");
            entity.Property(e => e.Other)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("other");
            entity.Property(e => e.PaymentTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("payment_total");
            entity.Property(e => e.ReceiverAddress)
                .HasColumnType("text")
                .HasColumnName("receiver_address");
            entity.Property(e => e.ReceiverName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("receiver_name");
            entity.Property(e => e.ReceiverTax)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("receiver_tax");
            entity.Property(e => e.SalaryTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("salary_total");
            entity.Property(e => e.ShoreTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("shore_total");
            entity.Property(e => e.TransportNet)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_net");
            entity.Property(e => e.TransportTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_total");
            entity.Property(e => e.TransportVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_vat");
            entity.Property(e => e.TripTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trip_total");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.TyreServiceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tyre_service_total");
            entity.Property(e => e.TyreTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tyre_total");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblFleetpaymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_supplyerpayment_fleetdetail");

            entity.ToTable("tbl_fleetpayment_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpressValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("express_value");
            entity.Property(e => e.FleetpaymentCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleetpayment_code");
            entity.Property(e => e.FuelLitreValue).HasColumnName("fuel_litre_value");
            entity.Property(e => e.FuelTotalValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fuel_total_value");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.PaymentValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("payment_value");
            entity.Property(e => e.TransportTotalValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_total_value");
            entity.Property(e => e.TransportValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_value");
            entity.Property(e => e.TransportVatValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_vat_value");
            entity.Property(e => e.TripValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trip_value");

            entity.HasOne(d => d.FleetpaymentCodeNavigation).WithMany(p => p.TblFleetpaymentDetails)
                .HasForeignKey(d => d.FleetpaymentCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_fleetpayment_detail_tbl_fleetpayment");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblFleetpaymentDetails)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_fleetpayment_detail_tbl_job_detail");
        });

        modelBuilder.Entity<TblFuelRate>(entity =>
        {
            entity.HasKey(e => new { e.ZoneCode, e.TruckTypegroupCode });

            entity.ToTable("tbl_fuel_rate");

            entity.Property(e => e.ZoneCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("zone_code");
            entity.Property(e => e.TruckTypegroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_typegroup_code");
            entity.Property(e => e.FuelRateValue).HasColumnName("fuel_rate_value");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.ZoneCodeNavigation).WithMany(p => p.TblFuelRates)
                .HasForeignKey(d => d.ZoneCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_fuelrate_tbl_zone");
        });

        modelBuilder.Entity<TblFuelType>(entity =>
        {
            entity.HasKey(e => e.FuelTypeCode);

            entity.ToTable("tbl_fuel_type");

            entity.Property(e => e.FuelTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fuel_type_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.FuelTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fuel_type_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblGroup>(entity =>
        {
            entity.HasKey(e => e.GroupCode);

            entity.ToTable("tbl_group");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.GroupName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("group_name");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblIndustry>(entity =>
        {
            entity.HasKey(e => e.IndustryCode);

            entity.ToTable("tbl_industry");

            entity.Property(e => e.IndustryCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("industry_code");
            entity.Property(e => e.IndustryAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("industry_address");
            entity.Property(e => e.IndustryFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("industry_fullname");
            entity.Property(e => e.IndustryLat)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("industry_lat");
            entity.Property(e => e.IndustryLon)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("industry_lon");
            entity.Property(e => e.IndustryNameTh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("industry_name_th");
            entity.Property(e => e.IndustryTax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("industry_tax");
            entity.Property(e => e.PortCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("port_code");
            entity.Property(e => e.ProvinceCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("province_code");
            entity.Property(e => e.ZoneCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("zone_code");

            entity.HasOne(d => d.IndustryCodeNavigation).WithOne(p => p.TblIndustry)
                .HasForeignKey<TblIndustry>(d => d.IndustryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_industry_tbl_place");

            entity.HasOne(d => d.ProvinceCodeNavigation).WithMany(p => p.TblIndustries)
                .HasForeignKey(d => d.ProvinceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_industry_lc_province");

            entity.HasOne(d => d.ZoneCodeNavigation).WithMany(p => p.TblIndustries)
                .HasForeignKey(d => d.ZoneCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_industry_tbl_zone");
        });

        modelBuilder.Entity<TblInsurance>(entity =>
        {
            entity.HasKey(e => e.InsuranceCode);

            entity.ToTable("tbl_insurance");

            entity.Property(e => e.InsuranceCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.InsuranceName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("insurance_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblInsuranceType>(entity =>
        {
            entity.HasKey(e => e.InsuranceTypeCode);

            entity.ToTable("tbl_insurance_type");

            entity.Property(e => e.InsuranceTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_type_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.InsuranceTypeName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("insurance_type_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblJobAllowance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_driver_cost");

            entity.ToTable("tbl_job_allowance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DriverAllowance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("driver_allowance");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.IsComplete).HasColumnName("is_complete");
            entity.Property(e => e.IsMonthlypayment).HasColumnName("is_monthlypayment");
            entity.Property(e => e.IsWeeklypayment).HasColumnName("is_weeklypayment");
            entity.Property(e => e.JobDate).HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.RoundTrip)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("round_trip");
            entity.Property(e => e.SupplierAllowance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplier_allowance");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobAllowances)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_allowance_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobAllowances)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_allowance_tbl_job_detail");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblJobAllowances)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_allowance_tbl_supplier");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobAllowances)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_allowance_tbl_truck");
        });

        modelBuilder.Entity<TblJobDetail>(entity =>
        {
            entity.HasKey(e => e.JobDetailCode);

            entity.ToTable("tbl_job_detail");

            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.FinishPlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("finish_place_code");
            entity.Property(e => e.IsApprove).HasColumnName("is_approve");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.IsDocument).HasColumnName("is_document");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true, "DF_tbl_job_detail_is_enabled")
                .HasColumnName("is_enabled");
            entity.Property(e => e.IsPayment).HasColumnName("is_payment");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("T", "DF_tbl_job_detail_job_status")
                .HasColumnName("job_status");
            entity.Property(e => e.MissionPlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mission_place_code");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.StartPlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("start_place_code");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierInvoice)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("supplier_invoice");
            entity.Property(e => e.TareWeight)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tare_weight");
            entity.Property(e => e.TrackingStatus)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("ST", "DF_tbl_job_detail_tracking_status")
                .HasColumnName("tracking_status");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobDetails)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_tbl_driver");

            entity.HasOne(d => d.FinishPlaceCodeNavigation).WithMany(p => p.TblJobDetailFinishPlaceCodeNavigations)
                .HasForeignKey(d => d.FinishPlaceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_finish_code");

            entity.HasOne(d => d.JobCodeNavigation).WithMany(p => p.TblJobDetails)
                .HasForeignKey(d => d.JobCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_tbl_job_master");

            entity.HasOne(d => d.MissionPlaceCodeNavigation).WithMany(p => p.TblJobDetailMissionPlaceCodeNavigations)
                .HasForeignKey(d => d.MissionPlaceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_mission_place");

            entity.HasOne(d => d.StartPlaceCodeNavigation).WithMany(p => p.TblJobDetailStartPlaceCodeNavigations)
                .HasForeignKey(d => d.StartPlaceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_start_place");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblJobDetails)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_tbl_supplier");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobDetails)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_detail_tbl_truck_fleet");
        });

        modelBuilder.Entity<TblJobImg>(entity =>
        {
            entity.ToTable("tbl_job_img");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobDetailImg).HasColumnName("job_detail_img");
            entity.Property(e => e.JobType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("job_type");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobImgs)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_img_tbl_job_detail");
        });

        modelBuilder.Entity<TblJobMaster>(entity =>
        {
            entity.HasKey(e => e.JobCode).HasName("PK_tbl_job_assignment");

            entity.ToTable("tbl_job_master");

            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.BookingPeriod)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("booking_period");
            entity.Property(e => e.ClosingTime)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_job_master_closing_time")
                .HasColumnName("closing_time");
            entity.Property(e => e.ContainerAmount).HasColumnName("container_amount");
            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.ContainerSizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_job_master_container_kind_name")
                .HasColumnName("container_size_name");
            entity.Property(e => e.CostCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cost_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerRemark)
                .HasColumnType("text")
                .HasColumnName("customer_remark");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobIncoterm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_job_master_job_incoterm")
                .HasColumnName("job_incoterm");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobType)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.LiftedAmount).HasColumnName("lifted_amount");
            entity.Property(e => e.LoadingDate)
                .HasColumnType("datetime")
                .HasColumnName("loading_date");
            entity.Property(e => e.OtherAddress)
                .HasColumnType("text")
                .HasColumnName("other_address");
            entity.Property(e => e.TradeType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("trade_type");
            entity.Property(e => e.TruckTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValueSql("((99))", "DF_tbl_job_master_truck_type_code")
                .HasColumnName("truck_type_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.ContainerSizeCodeNavigation).WithMany(p => p.TblJobMasters)
                .HasForeignKey(d => d.ContainerSizeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_master_tbl_container_kind");

            entity.HasOne(d => d.CostCodeNavigation).WithMany(p => p.TblJobMasters)
                .HasForeignKey(d => d.CostCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_master_tbl_customer_cost");

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.TblJobMasters)
                .HasForeignKey(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_master_tbl_customer");

            entity.HasOne(d => d.TruckTypeCodeNavigation).WithMany(p => p.TblJobMasters)
                .HasForeignKey(d => d.TruckTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_master_tbl_truck_type");
        });

        modelBuilder.Entity<TblJobOcean>(entity =>
        {
            entity.ToTable("tbl_job_ocean");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlNo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("bl_no");
            entity.Property(e => e.ChangeNo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("change_no");
            entity.Property(e => e.ConsigneeName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("consignee_name");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("delivery_date");
            entity.Property(e => e.Description)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EtaDate).HasColumnName("eta_date");
            entity.Property(e => e.GoodsDeclarationNo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_job_ocean_goods_declaration_no")
                .HasColumnName("goods_declaration_no");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.OrderCbm)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_cbm");
            entity.Property(e => e.OrderQuantity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_quantity");
            entity.Property(e => e.OrderWeight)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_weight");
            entity.Property(e => e.PickupDate)
                .HasColumnType("datetime")
                .HasColumnName("pickup_date");
            entity.Property(e => e.PlaceCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("place_code");
            entity.Property(e => e.RequestName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("request_name");
            entity.Property(e => e.ShipperName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("shipper_name");
            entity.Property(e => e.ShippingName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("shipping_name");
            entity.Property(e => e.VesselName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("vessel_name");

            entity.HasOne(d => d.JobCodeNavigation).WithMany(p => p.TblJobOceans)
                .HasForeignKey(d => d.JobCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_ocean_tbl_job_master");
        });

        modelBuilder.Entity<TblJobPlace>(entity =>
        {
            entity.ToTable("tbl_job_place");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FinishCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("finish_code");
            entity.Property(e => e.IsFinishyardport).HasColumnName("is_finishyardport");
            entity.Property(e => e.IsStartyardport).HasColumnName("is_startyardport");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.LiftoffValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("liftoff_value");
            entity.Property(e => e.LiftonValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifton_value");
            entity.Property(e => e.MissionCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mission_code");
            entity.Property(e => e.PortValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("port_value");
            entity.Property(e => e.StartCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_tbl_job_place_start_code")
                .HasColumnName("start_code");
            entity.Property(e => e.YardValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("yard_value");

            entity.HasOne(d => d.FinishCodeNavigation).WithMany(p => p.TblJobPlaceFinishCodeNavigations)
                .HasForeignKey(d => d.FinishCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_place_tbl_finish_place");

            entity.HasOne(d => d.JobCodeNavigation).WithMany(p => p.TblJobPlaces)
                .HasForeignKey(d => d.JobCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_place_tbl_job_master");

            entity.HasOne(d => d.StartCodeNavigation).WithMany(p => p.TblJobPlaceStartCodeNavigations)
                .HasForeignKey(d => d.StartCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_place_tbl_start_place");
        });

        modelBuilder.Entity<TblJobTracking>(entity =>
        {
            entity.ToTable("tbl_job_tracking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.Lattitude)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lattitude");
            entity.Property(e => e.Longitude)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("longitude");
            entity.Property(e => e.Speed)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("speed");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.TrackingDate)
                .HasColumnType("datetime")
                .HasColumnName("tracking_date");
            entity.Property(e => e.TrackingStatus)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tracking_status");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobTrackings)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_tracking_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobTrackings)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_tracking_tbl_job_detail");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblJobTrackings)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_tracking_tbl_supplier");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobTrackings)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_tracking_tbl_truck");
        });

        modelBuilder.Entity<TblJobTruck>(entity =>
        {
            entity.ToTable("tbl_job_truck");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.IsFinish).HasColumnName("is_finish");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobTrucks)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_truck_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobTrucks)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_truck_tbl_job_detail");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobTrucks)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_job_truck_tbl_truck");
        });

        modelBuilder.Entity<TblJobType>(entity =>
        {
            entity.HasKey(e => e.JobTypeCode);

            entity.ToTable("tbl_job_type");

            entity.Property(e => e.JobTypeCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_type_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.JobTypeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("job_type_name");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblJobadvance>(entity =>
        {
            entity.HasKey(e => e.AdvanceCode);

            entity.ToTable("tbl_jobadvance");

            entity.Property(e => e.AdvanceCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("advance_code");
            entity.Property(e => e.AdvanceLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoff");
            entity.Property(e => e.AdvanceLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_lifton");
            entity.Property(e => e.AdvancePort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_port");
            entity.Property(e => e.AdvanceRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("advance_remark");
            entity.Property(e => e.AdvanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_total");
            entity.Property(e => e.AdvanceYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yard");
            entity.Property(e => e.ClearingDate).HasColumnName("clearing_date");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsClearing).HasColumnName("is_clearing");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobadvances)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobadvance_tbl_job_detail");
        });

        modelBuilder.Entity<TblJobadvanceOther>(entity =>
        {
            entity.ToTable("tbl_jobadvance_other");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdvanceCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("advance_code");
            entity.Property(e => e.AdvanceOther)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("advance_other");
            entity.Property(e => e.AdvanceOtherValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_other_value");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.SupplierOtherValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplier_other_value");

            entity.HasOne(d => d.AdvanceCodeNavigation).WithMany(p => p.TblJobadvanceOthers)
                .HasForeignKey(d => d.AdvanceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobadvance_other_tbl_jobadvance");
        });

        modelBuilder.Entity<TblJobadvancePayment>(entity =>
        {
            entity.HasKey(e => e.JobDetailCode);

            entity.ToTable("tbl_jobadvance_payment");

            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.AdvanceValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_value");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.ReceiveDate)
                .HasColumnType("datetime")
                .HasColumnName("receive_date");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithOne(p => p.TblJobadvancePayment)
                .HasForeignKey<TblJobadvancePayment>(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobadvance_payment_tbl_job_detail");
        });

        modelBuilder.Entity<TblJobcashflow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_jobadvance_cashflow");

            entity.ToTable("tbl_jobcashflow");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdvanceRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("advance_remark");
            entity.Property(e => e.CashFlow)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cash_flow");
            entity.Property(e => e.ClearingDate)
                .HasColumnType("datetime")
                .HasColumnName("clearing_date");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.IsClearing).HasColumnName("is_clearing");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobcashflows)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobcashflows)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_tbl_job_detail");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobcashflows)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_tbl_truck");
        });

        modelBuilder.Entity<TblJobcashflowAdvance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_jobcashing_advance");

            entity.ToTable("tbl_jobcashflow_advance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdvanceLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoff");
            entity.Property(e => e.AdvanceLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_lifton");
            entity.Property(e => e.AdvancePort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_port");
            entity.Property(e => e.AdvanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_total");
            entity.Property(e => e.AdvanceYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yard");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobcashflowAdvances)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_advance_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobcashflowAdvances)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_advance_tbl_job_detail");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblJobcashflowAdvances)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_advance_tbl_supplier");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobcashflowAdvances)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_advance_tbl_truck");
        });

        modelBuilder.Entity<TblJobcashflowAdvanceOther>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_jobadvance_cashflow_detail");

            entity.ToTable("tbl_jobcashflow_advance_other");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdvanceOther)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("advance_other");
            entity.Property(e => e.AdvanceOtherValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_other_value");
            entity.Property(e => e.CashflowAdvanceId).HasColumnName("cashflow_advance_id");

            entity.HasOne(d => d.CashflowAdvance).WithMany(p => p.TblJobcashflowAdvanceOthers)
                .HasForeignKey(d => d.CashflowAdvanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_advance_other_tbl_jobcashflow_advance");
        });

        modelBuilder.Entity<TblJobcashflowCashing>(entity =>
        {
            entity.ToTable("tbl_jobcashflow_cashing");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cashing)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("cashing");
            entity.Property(e => e.CashingValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_value");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblJobcashflowCashings)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_cashing_tbl_driver");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobcashflowCashings)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_cashing_tbl_job_detail");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblJobcashflowCashings)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_cashing_tbl_supplier");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblJobcashflowCashings)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashflow_cashing_tbl_truck");
        });

        modelBuilder.Entity<TblJobcashing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_jobcashing_other");

            entity.ToTable("tbl_jobcashing");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cashing)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("cashing");
            entity.Property(e => e.CashingValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_value");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblJobcashings)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_jobcashing_tbl_job_detail");
        });

        modelBuilder.Entity<TblPlace>(entity =>
        {
            entity.HasKey(e => e.PlaceCode);

            entity.ToTable("tbl_place");

            entity.Property(e => e.PlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("place_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ExpresswayValue)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("expressway_value");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.PlaceName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_place_place_name_th")
                .HasColumnName("place_name");
            entity.Property(e => e.PlaceRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_place_place_remark")
                .HasColumnName("place_remark");
            entity.Property(e => e.PlaceType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("place_type");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblPortCashing>(entity =>
        {
            entity.HasKey(e => new { e.YardportCode, e.ContainerSizeCode });

            entity.ToTable("tbl_port_cashing");

            entity.Property(e => e.YardportCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("yardport_code");
            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.ContainerFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("container_fee");

            entity.HasOne(d => d.ContainerSizeCodeNavigation).WithMany(p => p.TblPortCashings)
                .HasForeignKey(d => d.ContainerSizeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_port_cashing_tbl_container_kind");

            entity.HasOne(d => d.YardportCodeNavigation).WithMany(p => p.TblPortCashings)
                .HasForeignKey(d => d.YardportCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_port_cashing_tbl_yardport");
        });

        modelBuilder.Entity<TblReceipt>(entity =>
        {
            entity.HasKey(e => e.ReceiptCode);

            entity.ToTable("tbl_receipt");

            entity.Property(e => e.ReceiptCode)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("receipt_code");
            entity.Property(e => e.BankBranchName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank_branch_name");
            entity.Property(e => e.BankName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("bank_name");
            entity.Property(e => e.ChequeNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cheque_no");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("customer_address");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerTax)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("customer_tax");
            entity.Property(e => e.ExchargeTopic)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("excharge_topic");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsExcharge).HasColumnName("is_excharge");
            entity.Property(e => e.ReceiptAdvance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("receipt_advance");
            entity.Property(e => e.ReceiptAmount).HasColumnName("receipt_amount");
            entity.Property(e => e.ReceiptDate).HasColumnName("receipt_date");
            entity.Property(e => e.ReceiptName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("receipt_name");
            entity.Property(e => e.ReceiptNetTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("receipt_net_total");
            entity.Property(e => e.ReceiptPartCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_receipt_receipt_part_code")
                .HasColumnName("receipt_part_code");
            entity.Property(e => e.ReceiptRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("receipt_remark");
            entity.Property(e => e.ReceiptTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("receipt_total");
            entity.Property(e => e.ReceiptTransport)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("receipt_transport");
            entity.Property(e => e.ReceiptType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("receipt_type");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.TblReceipts)
                .HasForeignKey(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_receipt_tbl_customer");
        });

        modelBuilder.Entity<TblReceiptType>(entity =>
        {
            entity.HasKey(e => e.ReceiptTypeCode);

            entity.ToTable("tbl_receipt_type");

            entity.Property(e => e.ReceiptTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("receipt_type_code");
            entity.Property(e => e.ReceiptTypeName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("receipt_type_name");
        });

        modelBuilder.Entity<TblRequest>(entity =>
        {
            entity.HasKey(e => e.RequestCode);

            entity.ToTable("tbl_request");

            entity.Property(e => e.RequestCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("request_code");
            entity.Property(e => e.RequestName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("request_name");
        });

        modelBuilder.Entity<TblShipper>(entity =>
        {
            entity.HasKey(e => e.ShipperCode);

            entity.ToTable("tbl_shipper");

            entity.Property(e => e.ShipperCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("shipper_code");
            entity.Property(e => e.ShipperName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("shipper_name");
        });

        modelBuilder.Entity<TblSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierCode).HasName("PK_tbl_supplyer");

            entity.ToTable("tbl_supplier");

            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.BankId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("bank_id");
            entity.Property(e => e.BankName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("bank_name");
            entity.Property(e => e.ContactName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("contact_name");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.FleetTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_type_code");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsSpecialVat).HasColumnName("is_special_vat");
            entity.Property(e => e.SupplierAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("supplier_address");
            entity.Property(e => e.SupplierFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("supplier_fullname");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("supplier_name");
            entity.Property(e => e.SupplierPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("supplier_phone");
            entity.Property(e => e.SupplierTypeCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_type_code");
            entity.Property(e => e.SupplierVatNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("supplier_vat_no");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblSupplierAllowance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_supplyer_cost_1");

            entity.ToTable("tbl_supplier_allowance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsComplete).HasColumnName("is_complete");
            entity.Property(e => e.IsPayment).HasColumnName("is_payment");
            entity.Property(e => e.JobDate).HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.SupplierAllowance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplier_allowance");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblSupplierAllowances)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplier_allowance_tbl_job_detail");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblSupplierAllowances)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplier_allowance_tbl_supplier_allowance");
        });

        modelBuilder.Entity<TblSupplierRate>(entity =>
        {
            entity.HasKey(e => new { e.SupplierCode, e.PlaceCode }).HasName("PK_tbl_supplier_cost_rate");

            entity.ToTable("tbl_supplier_rate");

            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.PlaceCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("place_code");
            entity.Property(e => e.CostRate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_rate");

            entity.HasOne(d => d.PlaceCodeNavigation).WithMany(p => p.TblSupplierRates)
                .HasForeignKey(d => d.PlaceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplier_rate_tbl_place");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblSupplierRates)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplier_rate_tbl_supplier");
        });

        modelBuilder.Entity<TblSupplierType>(entity =>
        {
            entity.HasKey(e => e.SupplierTypeCode).HasName("PK_tbl_supplyer_type");

            entity.ToTable("tbl_supplier_type");

            entity.Property(e => e.SupplierTypeCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_type_code");
            entity.Property(e => e.SupplierTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("supplier_type_name");
        });

        modelBuilder.Entity<TblSupplierpayment>(entity =>
        {
            entity.HasKey(e => e.SupplierpaymentCode).HasName("PK_tbl_supplyer_payment");

            entity.ToTable("tbl_supplierpayment");

            entity.Property(e => e.SupplierpaymentCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplierpayment_code");
            entity.Property(e => e.ChequeNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cheque_no");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsLiftoff).HasColumnName("is_liftoff");
            entity.Property(e => e.IsLifton).HasColumnName("is_lifton");
            entity.Property(e => e.IsPort).HasColumnName("is_port");
            entity.Property(e => e.IsYard).HasColumnName("is_yard");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("payment_type");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierpaymentAmount).HasColumnName("supplierpayment_amount");
            entity.Property(e => e.SupplierpaymentDate).HasColumnName("supplierpayment_date");
            entity.Property(e => e.SupplierpaymentNetTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplierpayment_net_total");
            entity.Property(e => e.SupplierpaymentRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("supplierpayment_remark");
            entity.Property(e => e.TotalAdditionalcharge)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_additionalcharge");
            entity.Property(e => e.TotalAdvance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_advance");
            entity.Property(e => e.TotalLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_liftoff");
            entity.Property(e => e.TotalLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_lifton");
            entity.Property(e => e.TotalPort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_port");
            entity.Property(e => e.TotalYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_yard");
            entity.Property(e => e.TransportNet)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_net");
            entity.Property(e => e.TransportTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_total");
            entity.Property(e => e.TransportVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transport_vat");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.SupplierCodeNavigation).WithMany(p => p.TblSupplierpayments)
                .HasForeignKey(d => d.SupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplierpayment_tbl_supplier");
        });

        modelBuilder.Entity<TblSupplierpaymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_supplyerpayment_detail");

            entity.ToTable("tbl_supplierpayment_detail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdditionalchargeValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("additionalcharge_value");
            entity.Property(e => e.AdvanceValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_value");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.LiftoffValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("liftoff_value");
            entity.Property(e => e.LiftonValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifton_value");
            entity.Property(e => e.PortValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("port_value");
            entity.Property(e => e.SupplierpaymentCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplierpayment_code");
            entity.Property(e => e.SupplierpaymentValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplierpayment_value");
            entity.Property(e => e.TotalValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_value");
            entity.Property(e => e.YardValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("yard_value");

            entity.HasOne(d => d.JobDetailCodeNavigation).WithMany(p => p.TblSupplierpaymentDetails)
                .HasForeignKey(d => d.JobDetailCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplierpayment_detail_tbl_job_detail");

            entity.HasOne(d => d.SupplierpaymentCodeNavigation).WithMany(p => p.TblSupplierpaymentDetails)
                .HasForeignKey(d => d.SupplierpaymentCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_supplierpayment_detail_tbl_supplierpayment");
        });

        modelBuilder.Entity<TblTrailer>(entity =>
        {
            entity.HasKey(e => e.TrailerCode);

            entity.ToTable("tbl_trailer");

            entity.Property(e => e.TrailerCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.TrailerFloorCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_floor_code");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TrailerNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_trailer_trailer_no")
                .HasColumnName("trailer_no");
            entity.Property(e => e.TrailerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_type_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.TrailerFloorCodeNavigation).WithMany(p => p.TblTrailers)
                .HasForeignKey(d => d.TrailerFloorCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_trailer_tbl_trailer_floor");

            entity.HasOne(d => d.TrailerTypeCodeNavigation).WithMany(p => p.TblTrailers)
                .HasForeignKey(d => d.TrailerTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_trailer_tbl_trailer_type");
        });

        modelBuilder.Entity<TblTrailerFloor>(entity =>
        {
            entity.HasKey(e => e.TrailerFloorCode);

            entity.ToTable("tbl_trailer_floor");

            entity.Property(e => e.TrailerFloorCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_floor_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.TrailerFloorName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trailer_floor_name");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblTrailerInsurance>(entity =>
        {
            entity.ToTable("tbl_trailer_insurance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ExpiredDate)
                .HasColumnType("datetime")
                .HasColumnName("expired_date");
            entity.Property(e => e.InsuranceBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_budget");
            entity.Property(e => e.InsuranceCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_code");
            entity.Property(e => e.InsuranceDetail)
                .HasColumnType("text")
                .HasColumnName("insurance_detail");
            entity.Property(e => e.InsuranceIncrease)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_increase");
            entity.Property(e => e.InsuranceTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_type_code");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.StartedDate)
                .HasColumnType("datetime")
                .HasColumnName("started_date");
            entity.Property(e => e.TrailerCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.InsuranceCodeNavigation).WithMany(p => p.TblTrailerInsurances)
                .HasForeignKey(d => d.InsuranceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_trailer_insurance_tbl_insurance");

            entity.HasOne(d => d.InsuranceTypeCodeNavigation).WithMany(p => p.TblTrailerInsurances)
                .HasForeignKey(d => d.InsuranceTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_trailer_insurance_tbl_trailer_type");

            entity.HasOne(d => d.TrailerCodeNavigation).WithMany(p => p.TblTrailerInsurances)
                .HasForeignKey(d => d.TrailerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_trailer_insurance_tbl_trailer");
        });

        modelBuilder.Entity<TblTrailerType>(entity =>
        {
            entity.HasKey(e => e.TrailerTypeCode);

            entity.ToTable("tbl_trailer_type");

            entity.Property(e => e.TrailerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_type_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.TrailerTypeName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("trailer_type_name");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<TblTruck>(entity =>
        {
            entity.HasKey(e => e.TruckCode).HasName("PK_tbl_truck_1");

            entity.ToTable("tbl_truck");

            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");

            entity.HasMany(d => d.FleetpaymentCodes).WithMany(p => p.TruckCodes)
                .UsingEntity<Dictionary<string, object>>(
                    "TblFleetpaymentTruck",
                    r => r.HasOne<TblFleetpayment>().WithMany()
                        .HasForeignKey("FleetpaymentCode")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_tbl_fleetpayment_truck_tbl_fleetpayment"),
                    l => l.HasOne<TblTruck>().WithMany()
                        .HasForeignKey("TruckCode")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_tbl_fleetpayment_truck_tbl_truck"),
                    j =>
                    {
                        j.HasKey("TruckCode", "FleetpaymentCode");
                        j.ToTable("tbl_fleetpayment_truck");
                        j.IndexerProperty<string>("TruckCode")
                            .HasMaxLength(7)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("truck_code");
                        j.IndexerProperty<string>("FleetpaymentCode")
                            .HasMaxLength(14)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("fleetpayment_code");
                    });
        });

        modelBuilder.Entity<TblTruckDetail>(entity =>
        {
            entity.HasKey(e => e.TruckCode).HasName("PK_tbl_truck");

            entity.ToTable("tbl_truck_detail");

            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.BrandCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("brand_code");
            entity.Property(e => e.FleetSupplierCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_code");
            entity.Property(e => e.FuelTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fuel_type_code");
            entity.Property(e => e.TruckChassisNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("truck_chassis_number");
            entity.Property(e => e.TruckImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("truck_image");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("-", "DF_tbl_truck_truck_no")
                .HasColumnName("truck_no");
            entity.Property(e => e.TruckTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_type_code");
            entity.Property(e => e.TruckTypegroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_typegroup_code");
            entity.Property(e => e.TruckYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_year");

            entity.HasOne(d => d.BrandCodeNavigation).WithMany(p => p.TblTruckDetails)
                .HasForeignKey(d => d.BrandCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_tbl_brand");

            entity.HasOne(d => d.FleetSupplierCodeNavigation).WithMany(p => p.TblTruckDetails)
                .HasForeignKey(d => d.FleetSupplierCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_detail_tbl_fleet_supplier");

            entity.HasOne(d => d.FuelTypeCodeNavigation).WithMany(p => p.TblTruckDetails)
                .HasForeignKey(d => d.FuelTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_tbl_fuel_type");

            entity.HasOne(d => d.TruckCodeNavigation).WithOne(p => p.TblTruckDetail)
                .HasForeignKey<TblTruckDetail>(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_detail_tbl_truck");

            entity.HasOne(d => d.TruckTypeCodeNavigation).WithMany(p => p.TblTruckDetails)
                .HasForeignKey(d => d.TruckTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_tbl_truck_type");
        });

        modelBuilder.Entity<TblTruckDriver>(entity =>
        {
            entity.ToTable("tbl_truck_driver");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");

            entity.HasOne(d => d.DriverCodeNavigation).WithMany(p => p.TblTruckDrivers)
                .HasForeignKey(d => d.DriverCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_driver_tbl_driver");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblTruckDrivers)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_driver_tbl_truck");
        });

        modelBuilder.Entity<TblTruckInsurance>(entity =>
        {
            entity.ToTable("tbl_truck_insurance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ExpiredDate)
                .HasColumnType("datetime")
                .HasColumnName("expired_date");
            entity.Property(e => e.InsuranceBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_budget");
            entity.Property(e => e.InsuranceCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_code");
            entity.Property(e => e.InsuranceDetail)
                .HasColumnType("text")
                .HasColumnName("insurance_detail");
            entity.Property(e => e.InsuranceIncrease)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_increase");
            entity.Property(e => e.InsuranceTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_type_code");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.StartedDate)
                .HasColumnType("datetime")
                .HasColumnName("started_date");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.InsuranceCodeNavigation).WithMany(p => p.TblTruckInsurances)
                .HasForeignKey(d => d.InsuranceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_insurance_tbl_insurance");

            entity.HasOne(d => d.InsuranceTypeCodeNavigation).WithMany(p => p.TblTruckInsurances)
                .HasForeignKey(d => d.InsuranceTypeCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_insurance_tbl_insurance_type");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblTruckInsurances)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_insurance_tbl_truck");
        });

        modelBuilder.Entity<TblTruckTrailer>(entity =>
        {
            entity.ToTable("tbl_truck_trailer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.TrailerCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_code");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.TrailerCodeNavigation).WithMany(p => p.TblTruckTrailers)
                .HasForeignKey(d => d.TrailerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_trailer_tbl_trailer");

            entity.HasOne(d => d.TruckCodeNavigation).WithMany(p => p.TblTruckTrailers)
                .HasForeignKey(d => d.TruckCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_truck_trailer_tbl_truck");
        });

        modelBuilder.Entity<TblTruckType>(entity =>
        {
            entity.HasKey(e => e.TruckTypeCode);

            entity.ToTable("tbl_truck_type");

            entity.Property(e => e.TruckTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_type_code");
            entity.Property(e => e.TruckTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("truck_type_name");
        });

        modelBuilder.Entity<TblYardCashing>(entity =>
        {
            entity.HasKey(e => e.YardportCode);

            entity.ToTable("tbl_yard_cashing");

            entity.Property(e => e.YardportCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("yardport_code");
            entity.Property(e => e.LongliftoffFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("longliftoff_fee");
            entity.Property(e => e.LongliftonFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("longlifton_fee");
            entity.Property(e => e.ShortliftoffFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("shortliftoff_fee");
            entity.Property(e => e.ShortliftonFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("shortlifton_fee");

            entity.HasOne(d => d.YardportCodeNavigation).WithOne(p => p.TblYardCashing)
                .HasForeignKey<TblYardCashing>(d => d.YardportCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_yard_cashing_tbl_yardport");
        });

        modelBuilder.Entity<TblYardport>(entity =>
        {
            entity.HasKey(e => e.YardportCode);

            entity.ToTable("tbl_yardport");

            entity.Property(e => e.YardportCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("yardport_code");
            entity.Property(e => e.ExportFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("export_fee");
            entity.Property(e => e.ImportFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("import_fee");

            entity.HasOne(d => d.YardportCodeNavigation).WithOne(p => p.TblYardport)
                .HasForeignKey<TblYardport>(d => d.YardportCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_yardport_tbl_place");
        });

        modelBuilder.Entity<TblZone>(entity =>
        {
            entity.HasKey(e => e.ZoneCode);

            entity.ToTable("tbl_zone");

            entity.Property(e => e.ZoneCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("zone_code");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("zone_name");
        });

        modelBuilder.Entity<ViewBillingListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewBillingListActive");

            entity.Property(e => e.AdvanceLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoff");
            entity.Property(e => e.AdvanceLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_lifton");
            entity.Property(e => e.AdvancePort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_port");
            entity.Property(e => e.AdvanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_total");
            entity.Property(e => e.AdvanceValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_value");
            entity.Property(e => e.AdvanceYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yard");
            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.BillingAmount).HasColumnName("billing_amount");
            entity.Property(e => e.BillingCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("billing_code");
            entity.Property(e => e.BillingDate).HasColumnName("billing_date");
            entity.Property(e => e.BillingRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("billing_remark");
            entity.Property(e => e.BillingTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("billing_total");
            entity.Property(e => e.BillingType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("billing_type");
            entity.Property(e => e.BillingValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("billing_value");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.CashingTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_total");
            entity.Property(e => e.ClearingDate).HasColumnName("clearing_date");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSize)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size");
            entity.Property(e => e.ContainerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_type");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("customer_address");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_type_code");
            entity.Property(e => e.CustomerTypeName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("customer_type_name");
            entity.Property(e => e.FinishName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finish_name");
            entity.Property(e => e.IsAdvance).HasColumnName("is_advance");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsClearing).HasColumnName("is_clearing");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsVat).HasColumnName("is_vat");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("job_date");
            entity.Property(e => e.JobDateValue)
                .HasColumnType("datetime")
                .HasColumnName("job_date_value");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_status");
            entity.Property(e => e.JobType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.StartName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start_name");
            entity.Property(e => e.TaxAddress)
                .HasColumnType("text")
                .HasColumnName("tax_address");
            entity.Property(e => e.TaxId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tax_id");
            entity.Property(e => e.TaxName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("tax_name");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
        });

        modelBuilder.Entity<ViewBillingOtherListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewBillingOtherListActive");

            entity.Property(e => e.AdvanceLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoff");
            entity.Property(e => e.AdvanceLiftoffup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoffup");
            entity.Property(e => e.AdvanceLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_lifton");
            entity.Property(e => e.AdvanceLiftonup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftonup");
            entity.Property(e => e.AdvanceOther)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("advance_other");
            entity.Property(e => e.AdvanceOtherValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_other_value");
            entity.Property(e => e.AdvanceOtherValueup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_other_valueup");
            entity.Property(e => e.AdvancePort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_port");
            entity.Property(e => e.AdvancePortup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_portup");
            entity.Property(e => e.AdvanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_total");
            entity.Property(e => e.AdvanceTotalup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_totalup");
            entity.Property(e => e.AdvanceYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yard");
            entity.Property(e => e.AdvanceYardup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yardup");
            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.BillingAmount).HasColumnName("billing_amount");
            entity.Property(e => e.BillingCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("billing_code");
            entity.Property(e => e.BillingDate).HasColumnName("billing_date");
            entity.Property(e => e.BillingRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("billing_remark");
            entity.Property(e => e.BillingTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("billing_total");
            entity.Property(e => e.BillingType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("billing_type");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.CashingTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_total");
            entity.Property(e => e.ClearingDate).HasColumnName("clearing_date");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSize)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size");
            entity.Property(e => e.ContainerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_type");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("customer_address");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.CustomerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_type_code");
            entity.Property(e => e.CustomerTypeName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("customer_type_name");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.FinishName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finish_name");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsAdvance).HasColumnName("is_advance");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsClearing).HasColumnName("is_clearing");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsVat).HasColumnName("is_vat");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("job_date");
            entity.Property(e => e.JobDateValue)
                .HasColumnType("datetime")
                .HasColumnName("job_date_value");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_status");
            entity.Property(e => e.JobType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.StartName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start_name");
            entity.Property(e => e.TaxAddress)
                .HasColumnType("text")
                .HasColumnName("tax_address");
            entity.Property(e => e.TaxId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tax_id");
            entity.Property(e => e.TaxName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("tax_name");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
        });

        modelBuilder.Entity<ViewCostListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewCostListActive");

            entity.Property(e => e.CostCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cost_code");
            entity.Property(e => e.CostRemark)
                .HasColumnType("text")
                .HasColumnName("cost_remark");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.ExpresswayValue)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("expressway_value");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.JobName)
                .HasMaxLength(150)
                .HasColumnName("job_name");
            entity.Property(e => e.MissionCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mission_code");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.PlaceType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("place_type");
            entity.Property(e => e.ReceiptAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("receipt_address");
            entity.Property(e => e.ReceiptName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("receipt_name");
            entity.Property(e => e.ReceiptTax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("receipt_tax");
        });

        modelBuilder.Entity<ViewCoverSheetListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewCoverSheetListActive");

            entity.Property(e => e.BillingCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("billing_code");
            entity.Property(e => e.CoversheetAmount).HasColumnName("coversheet_amount");
            entity.Property(e => e.CoversheetCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coversheet_code");
            entity.Property(e => e.CoversheetValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("coversheet_value");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name");
        });

        modelBuilder.Entity<ViewDriverListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewDriverListActive");

            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.FleetSupplierTypeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_type_code");
            entity.Property(e => e.FleetSupplierTypeName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("fleet_supplier_type_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("truck_no");
            entity.Property(e => e.TruckTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("truck_type_name");
            entity.Property(e => e.TruckTypegroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_typegroup_code");
            entity.Property(e => e.TruckTypegroupName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("truck_typegroup_name");
        });

        modelBuilder.Entity<ViewDriverPaymentListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewDriverPaymentListActive");

            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.ContainerSizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size_name");
            entity.Property(e => e.CostType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cost_type");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.FuelBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fuel_balance");
            entity.Property(e => e.FuelUp).HasColumnName("fuel_up");
            entity.Property(e => e.FuelValue).HasColumnName("fuel_value");
            entity.Property(e => e.HashCode)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("hash_code");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.OtherValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("other_value");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.StartName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start_name");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TripValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("trip_value");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
            entity.Property(e => e.TruckTypegroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_typegroup_code");
            entity.Property(e => e.WasteValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("waste_value");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("zone_name");
        });

        modelBuilder.Entity<ViewEmployeeListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewEmployeeListActive");

            entity.Property(e => e.EmpCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("emp_code");
            entity.Property(e => e.EmpFullname)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("emp_fullname");
            entity.Property(e => e.EmpMail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emp_mail");
            entity.Property(e => e.EmpPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emp_phone");
            entity.Property(e => e.IdCard)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("id_card");
            entity.Property(e => e.IdLine)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_line");
            entity.Property(e => e.InitialName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("initial_name");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
        });

        modelBuilder.Entity<ViewGeneralBudgetTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewGeneralBudgetTransaction");

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AmountDiff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount_diff");
            entity.Property(e => e.ApproveStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("approve_status");
            entity.Property(e => e.BudgetCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("budget_code");
            entity.Property(e => e.BudgetType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("budget_type");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.DailypaymentCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dailypayment_code");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("payment_type");
            entity.Property(e => e.Remark)
                .HasColumnType("text")
                .HasColumnName("remark");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("transaction_code");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("transaction_type");
        });

        modelBuilder.Entity<ViewGroupPriviledgeLevel1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewGroupPriviledgeLevel1");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.Level01Active)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_01_active");
            entity.Property(e => e.Level01Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_01_code");
            entity.Property(e => e.Level01Icon)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_01_icon");
            entity.Property(e => e.Level01Index).HasColumnName("level_01_index");
            entity.Property(e => e.Level01Link)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("level_01_link");
            entity.Property(e => e.Level01Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_01_name");
        });

        modelBuilder.Entity<ViewGroupPriviledgeLevel2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewGroupPriviledgeLevel2");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.Level01Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_01_code");
            entity.Property(e => e.Level02Active)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_02_active");
            entity.Property(e => e.Level02Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_02_code");
            entity.Property(e => e.Level02Index).HasColumnName("level_02_index");
            entity.Property(e => e.Level02Link)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("level_02_link");
            entity.Property(e => e.Level02Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_02_name");
        });

        modelBuilder.Entity<ViewGroupPriviledgeLevel3>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewGroupPriviledgeLevel3");

            entity.Property(e => e.GroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("group_code");
            entity.Property(e => e.Level02Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_02_code");
            entity.Property(e => e.Level03Active)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("level_03_active");
            entity.Property(e => e.Level03Code)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("level_03_code");
            entity.Property(e => e.Level03Link)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("level_03_link");
            entity.Property(e => e.Level03Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_03_name");
        });

        modelBuilder.Entity<ViewJobAdvanceCashingListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewJobAdvanceCashingListActive");

            entity.Property(e => e.AdvanceCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("advance_code");
            entity.Property(e => e.AdvanceLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoff");
            entity.Property(e => e.AdvanceLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_lifton");
            entity.Property(e => e.AdvanceOtherValue)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("advance_other_value");
            entity.Property(e => e.AdvancePort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_port");
            entity.Property(e => e.AdvanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_total");
            entity.Property(e => e.AdvanceYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yard");
            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.BillingType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("billing_type");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.CashingCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cashing_code");
            entity.Property(e => e.CashingExpressway)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_expressway");
            entity.Property(e => e.CashingExpresswayup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_expresswayup");
            entity.Property(e => e.CashingFuel).HasColumnName("cashing_fuel");
            entity.Property(e => e.CashingFuelup).HasColumnName("cashing_fuelup");
            entity.Property(e => e.CashingOther)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_other");
            entity.Property(e => e.CashingOtherValue)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("cashing_other_value");
            entity.Property(e => e.CashingTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_total");
            entity.Property(e => e.CashingWaste)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_waste");
            entity.Property(e => e.ClearingDate).HasColumnName("clearing_date");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size_name");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.CustomerNameEn)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_en");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.Industrial)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("industrial");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.IsClearing).HasColumnName("is_clearing");
            entity.Property(e => e.IsDocument).HasColumnName("is_document");
            entity.Property(e => e.IsSingleTrip).HasColumnName("is_single_trip");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.Shore)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("shore");
            entity.Property(e => e.Start)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("supplier_name");
            entity.Property(e => e.TareWeight)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tare_weight");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("truck_no");
        });

        modelBuilder.Entity<ViewJobCashingListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewJobCashingListActive");

            entity.Property(e => e.AdvanceLiftoff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_liftoff");
            entity.Property(e => e.AdvanceLifton)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_lifton");
            entity.Property(e => e.AdvancePort)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_port");
            entity.Property(e => e.AdvanceTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_total");
            entity.Property(e => e.AdvanceYard)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("advance_yard");
            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.BillingDate).HasColumnName("billing_date");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.ClearingDate).HasColumnName("clearing_date");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSize)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size");
            entity.Property(e => e.ContainerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_type");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.CustomerNameEn)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_en");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsClearing).HasColumnName("is_clearing");
            entity.Property(e => e.IsDocument).HasColumnName("is_document");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("supplier_name");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
        });

        modelBuilder.Entity<ViewJobListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewJobListActive");

            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.AgentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("agent_name");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.CashFlow)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cash_flow");
            entity.Property(e => e.ClosingTime)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("closing_time");
            entity.Property(e => e.ContainerAmount).HasColumnName("container_amount");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.ContainerSizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size_name");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.CustomerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_type_code");
            entity.Property(e => e.DriverAllowance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("driver_allowance");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.FinishCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("finish_code");
            entity.Property(e => e.FinishName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finish_name");
            entity.Property(e => e.FleetTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_type_code");
            entity.Property(e => e.HashCode)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("hash_code");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsApprove).HasColumnName("is_approve");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.IsDocument).HasColumnName("is_document");
            entity.Property(e => e.IsLiftCashing).HasColumnName("is_lift_cashing");
            entity.Property(e => e.IsWeeklypayment).HasColumnName("is_weeklypayment");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(20)
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobName)
                .HasMaxLength(150)
                .HasColumnName("job_name");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_status");
            entity.Property(e => e.JobType)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.LiftedAmount).HasColumnName("lifted_amount");
            entity.Property(e => e.LoadingDate)
                .HasColumnType("datetime")
                .HasColumnName("loading_date");
            entity.Property(e => e.MissionCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mission_code");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.ReceiptAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("receipt_address");
            entity.Property(e => e.ReceiptName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("receipt_name");
            entity.Property(e => e.ReceiptTax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("receipt_tax");
            entity.Property(e => e.RoundTrip)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("round_trip");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.StartCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("start_code");
            entity.Property(e => e.StartName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start_name");
            entity.Property(e => e.SupplierAllowance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplier_allowance");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierInvoice)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("supplier_invoice");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("supplier_name");
            entity.Property(e => e.TareWeight)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tare_weight");
            entity.Property(e => e.TrackingStatus)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tracking_status");
            entity.Property(e => e.TradeType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("trade_type");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TripcostRule).HasColumnName("tripcost_rule");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("truck_no");
            entity.Property(e => e.TruckTypegroupName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("truck_typegroup_name");
        });

        modelBuilder.Entity<ViewJobSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewJobSummary");

            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.CashingTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cashing_total");
            entity.Property(e => e.ClosingTime)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("closing_time");
            entity.Property(e => e.ContainerAmount).HasColumnName("container_amount");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.ContainerSize)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size");
            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.CostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("cost_value");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.CustomerTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_type_code");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.FinishCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("finish_code");
            entity.Property(e => e.FinishName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finish_name");
            entity.Property(e => e.IsApprove).HasColumnName("is_approve");
            entity.Property(e => e.IsBilling).HasColumnName("is_billing");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.IsDocument).HasColumnName("is_document");
            entity.Property(e => e.IsSingleTrip).HasColumnName("is_single_trip");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_status");
            entity.Property(e => e.JobType)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.LiftedAmount).HasColumnName("lifted_amount");
            entity.Property(e => e.LoadingDate)
                .HasColumnType("datetime")
                .HasColumnName("loading_date");
            entity.Property(e => e.MissionCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mission_code");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.SealNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seal_no");
            entity.Property(e => e.StartCode)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("start_code");
            entity.Property(e => e.StartName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start_name");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierCostValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("supplier_cost_value");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("supplier_name");
            entity.Property(e => e.TareWeight)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tare_weight");
            entity.Property(e => e.TrackingStatus)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tracking_status");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
        });

        modelBuilder.Entity<ViewOrderListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewOrderListActive");

            entity.Property(e => e.AgentCode)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("agent_code");
            entity.Property(e => e.AgentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("agent_name");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.ClosingTime)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("closing_time");
            entity.Property(e => e.ContainerAmount).HasColumnName("container_amount");
            entity.Property(e => e.ContainerSizeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("container_size_code");
            entity.Property(e => e.ContainerSizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_size_name");
            entity.Property(e => e.CustomerNameTh)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name_th");
            entity.Property(e => e.HashCode)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("hash_code");
            entity.Property(e => e.IndustryFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("industry_fullname");
            entity.Property(e => e.IndustryNameTh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("industry_name_th");
            entity.Property(e => e.Is2short).HasColumnName("is_2short");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.IsLiftCashing).HasColumnName("is_lift_cashing");
            entity.Property(e => e.JobCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_code");
            entity.Property(e => e.JobDate)
                .HasColumnType("datetime")
                .HasColumnName("job_date");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobInvoice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("job_invoice");
            entity.Property(e => e.JobName)
                .HasMaxLength(150)
                .HasColumnName("job_name");
            entity.Property(e => e.JobRemark)
                .HasColumnType("text")
                .HasColumnName("job_remark");
            entity.Property(e => e.JobType)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.LiftoffName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("liftoff_name");
            entity.Property(e => e.LiftoffValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("liftoff_value");
            entity.Property(e => e.LiftonName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lifton_name");
            entity.Property(e => e.LiftonValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("lifton_value");
            entity.Property(e => e.LoadingDate)
                .HasColumnType("datetime")
                .HasColumnName("loading_date");
            entity.Property(e => e.MissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission_name");
            entity.Property(e => e.OtherAddress)
                .HasColumnType("text")
                .HasColumnName("other_address");
            entity.Property(e => e.PlaceName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("place_name");
            entity.Property(e => e.PortCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("port_code");
            entity.Property(e => e.PortValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("port_value");
            entity.Property(e => e.ReceiptAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("receipt_address");
            entity.Property(e => e.ReceiptName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("receipt_name");
            entity.Property(e => e.ReceiptTax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("receipt_tax");
            entity.Property(e => e.TruckTypegroupCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_typegroup_code");
            entity.Property(e => e.YardValue)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("yard_value");
        });

        modelBuilder.Entity<ViewTrailerInsuranceList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewTrailerInsuranceList");

            entity.Property(e => e.ExpiredDate)
                .HasColumnType("datetime")
                .HasColumnName("expired_date");
            entity.Property(e => e.InsuranceBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_budget");
            entity.Property(e => e.InsuranceCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_code");
            entity.Property(e => e.InsuranceDetail)
                .HasColumnType("text")
                .HasColumnName("insurance_detail");
            entity.Property(e => e.InsuranceIncrease)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_increase");
            entity.Property(e => e.InsuranceName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("insurance_name");
            entity.Property(e => e.InsuranceTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_type_code");
            entity.Property(e => e.InsuranceTypeName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("insurance_type_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.StartedDate)
                .HasColumnType("datetime")
                .HasColumnName("started_date");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
        });

        modelBuilder.Entity<ViewTrailerListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewTrailerListActive");

            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.TrailerCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("trailer_code");
            entity.Property(e => e.TrailerFloorName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trailer_floor_name");
            entity.Property(e => e.TrailerLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("trailer_license");
            entity.Property(e => e.TrailerNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("trailer_no");
            entity.Property(e => e.TrailerTypeName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("trailer_type_name");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
        });

        modelBuilder.Entity<ViewTransportBudgetTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewTransportBudgetTransaction");

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AmountDiff)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount_diff");
            entity.Property(e => e.BookingNo)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("booking_no");
            entity.Property(e => e.BudgetCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("budget_code");
            entity.Property(e => e.BudgetType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("budget_type");
            entity.Property(e => e.ContainerNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_no");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("create_by");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CustomerAcronym)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("customer_acronym");
            entity.Property(e => e.JobDetailCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("job_detail_code");
            entity.Property(e => e.JobType)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("job_type");
            entity.Property(e => e.Mission)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mission");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("payment_type");
            entity.Property(e => e.Remark)
                .HasColumnType("text")
                .HasColumnName("remark");
            entity.Property(e => e.Start)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("start");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("transaction_code");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("transaction_type");
        });

        modelBuilder.Entity<ViewTruckInsuranceListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewTruckInsuranceListActive");

            entity.Property(e => e.ExpiredDate)
                .HasColumnType("datetime")
                .HasColumnName("expired_date");
            entity.Property(e => e.InsuranceBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_budget");
            entity.Property(e => e.InsuranceCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_code");
            entity.Property(e => e.InsuranceDetail)
                .HasColumnType("text")
                .HasColumnName("insurance_detail");
            entity.Property(e => e.InsuranceIncrease)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("insurance_increase");
            entity.Property(e => e.InsuranceName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("insurance_name");
            entity.Property(e => e.InsuranceTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("insurance_type_code");
            entity.Property(e => e.InsuranceTypeName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("insurance_type_name");
            entity.Property(e => e.IsEnabled).HasColumnName("is_enabled");
            entity.Property(e => e.StartedDate)
                .HasColumnType("datetime")
                .HasColumnName("started_date");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
        });

        modelBuilder.Entity<ViewTruckListActive>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("viewTruckListActive");

            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("brand_name");
            entity.Property(e => e.DriverCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("driver_code");
            entity.Property(e => e.DriverFullname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("driver_fullname");
            entity.Property(e => e.DriverPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("driver_phone");
            entity.Property(e => e.FleetSupplierCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_code");
            entity.Property(e => e.FleetSupplierName)
                .HasMaxLength(250)
                .HasColumnName("fleet_supplier_name");
            entity.Property(e => e.FleetSupplierTypeCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("fleet_supplier_type_code");
            entity.Property(e => e.FleetSupplierTypeName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("fleet_supplier_type_name");
            entity.Property(e => e.FuelTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fuel_type_name");
            entity.Property(e => e.IsActived).HasColumnName("is_actived");
            entity.Property(e => e.TruckChassisNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("truck_chassis_number");
            entity.Property(e => e.TruckCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_code");
            entity.Property(e => e.TruckImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("truck_image");
            entity.Property(e => e.TruckLicense)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("truck_license");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("truck_no");
            entity.Property(e => e.TruckTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("truck_type_name");
            entity.Property(e => e.TruckYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("truck_year");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
