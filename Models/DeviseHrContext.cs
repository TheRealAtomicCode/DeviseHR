using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models;

public partial class DeviseHrContext : DbContext
{
    public DeviseHrContext()
    {
    }

    public DeviseHrContext(DbContextOptions<DeviseHrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Absence> Absences { get; set; }

    public virtual DbSet<AbsenceType> AbsenceTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Hierarchy> Hierarchies { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Operator> Operators { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<WorkingPattern> WorkingPatterns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=DeviseHR;Username=postgres;Password=890899000");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("absence_pkey");

            entity.ToTable("absence");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbsenceEndDate).HasColumnName("absence_end_date");
            entity.Property(e => e.AbsenceStartDate).HasColumnName("absence_start_date");
            entity.Property(e => e.AbsenceTypeId).HasColumnName("absence_type_id");
            entity.Property(e => e.AddedBy).HasColumnName("added_by");
            entity.Property(e => e.ApprovedByAdmin).HasColumnName("approved_by_admin");
            entity.Property(e => e.ApprovedId).HasColumnName("approved_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.ContractId).HasColumnName("contract_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DaysDeducted).HasColumnName("days_deducted");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.HoursDeducted).HasColumnName("hours_deducted");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasColumnName("is_approved");
            entity.Property(e => e.IsDays)
                .HasDefaultValue(true)
                .HasColumnName("is_days");
            entity.Property(e => e.IsFirstHalfDay).HasColumnName("is_first_half_day");
            entity.Property(e => e.IsPending)
                .HasDefaultValue(true)
                .HasColumnName("is_pending");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.AbsenceType).WithMany(p => p.Absences)
                .HasForeignKey(d => d.AbsenceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_absence_type");

            entity.HasOne(d => d.ApprovedByAdminNavigation).WithMany(p => p.AbsenceApprovedByAdminNavigations)
                .HasForeignKey(d => d.ApprovedByAdmin)
                .HasConstraintName("fk_approved_by_admin");

            entity.HasOne(d => d.Approved).WithMany(p => p.AbsenceApproveds)
                .HasForeignKey(d => d.ApprovedId)
                .HasConstraintName("fk_approved_by_manager");

            entity.HasOne(d => d.Company).WithMany(p => p.Absences)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_company");

            entity.HasOne(d => d.Contract).WithMany(p => p.Absences)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract");

            entity.HasOne(d => d.Employee).WithMany(p => p.AbsenceEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee");
        });

        modelBuilder.Entity<AbsenceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("absence_type_pkey");

            entity.ToTable("absence_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbsenceName)
                .HasMaxLength(60)
                .HasColumnName("absence_name");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_pkey");

            entity.ToTable("company");

            entity.HasIndex(e => e.AccountNumber, "company_account_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(6)
                .HasColumnName("account_number");
            entity.Property(e => e.AddedByOperator).HasColumnName("added_by_operator");
            entity.Property(e => e.AnnualLeaveStartDate)
                .HasDefaultValueSql("'1970-01-01'::date")
                .HasColumnName("annual_leave_start_date");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(60)
                .HasColumnName("company_name");
            entity.Property(e => e.Country)
                .HasMaxLength(5)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EnableAbsenceConflictsOutsideDepartments)
                .HasDefaultValue(false)
                .HasColumnName("enable_absence_conflicts_outside_departments");
            entity.Property(e => e.EnableAcceptDeclineShifts)
                .HasDefaultValue(false)
                .HasColumnName("enable_accept_decline_shifts");
            entity.Property(e => e.EnableBroadcastShiftSwap)
                .HasDefaultValue(false)
                .HasColumnName("enable_broadcast_shift_swap");
            entity.Property(e => e.EnableCarryover)
                .HasDefaultValue(false)
                .HasColumnName("enable_carryover");
            entity.Property(e => e.EnableEditMyInformation)
                .HasDefaultValue(false)
                .HasColumnName("enable_edit_my_information");
            entity.Property(e => e.EnableOvertime)
                .HasDefaultValue(false)
                .HasColumnName("enable_overtime");
            entity.Property(e => e.EnableRequireTwoStageApproval)
                .HasDefaultValue(false)
                .HasColumnName("enable_require_two_stage_approval");
            entity.Property(e => e.EnableSelfCancelLeaveRequests)
                .HasDefaultValue(false)
                .HasColumnName("enable_self_cancel_leave_requests");
            entity.Property(e => e.EnableSemiPersonalInformation)
                .HasDefaultValue(false)
                .HasColumnName("enable_semi_personal_information");
            entity.Property(e => e.EnableShowEmployees)
                .HasDefaultValue(false)
                .HasColumnName("enable_show_employees");
            entity.Property(e => e.EnableTakeoverShift)
                .HasDefaultValue(false)
                .HasColumnName("enable_takeover_shift");
            entity.Property(e => e.EnableToil)
                .HasDefaultValue(false)
                .HasColumnName("enable_toil");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiration_date");
            entity.Property(e => e.IsSpecialClient)
                .HasDefaultValue(false)
                .HasColumnName("is_special_client");
            entity.Property(e => e.Lang)
                .HasMaxLength(5)
                .HasColumnName("lang");
            entity.Property(e => e.LicenceNumber)
                .HasMaxLength(255)
                .HasColumnName("licence_number");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.MainContactId).HasColumnName("main_contact_id");
            entity.Property(e => e.MaxEmployeesAllowed).HasColumnName("max_employees_allowed");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(14)
                .HasColumnName("phone_number");
            entity.Property(e => e.SecurityAnswerOne)
                .HasMaxLength(60)
                .HasColumnName("security_answer_one");
            entity.Property(e => e.SecurityAnswerTwo)
                .HasMaxLength(60)
                .HasColumnName("security_answer_two");
            entity.Property(e => e.SecurityQuestionOne)
                .HasMaxLength(60)
                .HasColumnName("security_question_one");
            entity.Property(e => e.SecurityQuestionTwo)
                .HasMaxLength(60)
                .HasColumnName("security_question_two");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByOperator).HasColumnName("updated_by_operator");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contract_pkey");

            entity.ToTable("contract");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedBy).HasColumnName("added_by");
            entity.Property(e => e.AverageWorkingDay)
                .HasDefaultValue(480)
                .HasColumnName("average_working_day");
            entity.Property(e => e.CompanyDaysPerWeek)
                .HasDefaultValue(0)
                .HasColumnName("company_days_per_week");
            entity.Property(e => e.CompanyHoursPerWeek).HasColumnName("company_hours_per_week");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CompanyLeaveEntitlement)
                .HasDefaultValue(0)
                .HasColumnName("company_leave_entitlement");
            entity.Property(e => e.ContractStartDate).HasColumnName("contract_start_date");
            entity.Property(e => e.ContractType).HasColumnName("contract_type");
            entity.Property(e => e.ContractedDaysPerWeek)
                .HasDefaultValue(0)
                .HasColumnName("contracted_days_per_week");
            entity.Property(e => e.ContractedHoursPerWeek)
                .HasDefaultValue(480)
                .HasColumnName("contracted_hours_per_week");
            entity.Property(e => e.ContractedLeaveEntitlement)
                .HasDefaultValue(0)
                .HasColumnName("contracted_leave_entitlement");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DiscardedId).HasColumnName("discarded_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.FirstLeaveAllowence)
                .HasDefaultValue(0)
                .HasColumnName("first_leave_allowence");
            entity.Property(e => e.IsDays)
                .HasDefaultValue(true)
                .HasColumnName("is_days");
            entity.Property(e => e.NextLeaveAllowence)
                .HasDefaultValue(0)
                .HasColumnName("next_leave_allowence");
            entity.Property(e => e.PatternId).HasColumnName("pattern_id");
            entity.Property(e => e.TermTimeId).HasColumnName("term_time_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_company_id_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_employee_id_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.Email, "employee_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedByOperator).HasColumnName("added_by_operator");
            entity.Property(e => e.AddedByUser).HasColumnName("added_by_user");
            entity.Property(e => e.AnnualLeaveStartDate).HasColumnName("annual_leave_start_date");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.ContractedLeaveStartDate).HasColumnName("contracted_leave_start_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DriversLicenceExpirationDate).HasColumnName("drivers_licence_expiration_date");
            entity.Property(e => e.DriversLicenceNumber)
                .HasMaxLength(60)
                .HasColumnName("drivers_licence_number");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.EnableBirthdayReminder)
                .HasDefaultValue(false)
                .HasColumnName("enable_birthday_reminder");
            entity.Property(e => e.EnableReceiveRequests)
                .HasDefaultValue(false)
                .HasColumnName("enable_receive_requests");
            entity.Property(e => e.EnableReceiveRequestsFromMyDepartment)
                .HasDefaultValue(false)
                .HasColumnName("enable_receive_requests_from_my_department");
            entity.Property(e => e.EnableReminders)
                .HasDefaultValue(false)
                .HasColumnName("enable_reminders");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.IsTerminated)
                .HasDefaultValue(false)
                .HasColumnName("is_terminated");
            entity.Property(e => e.IsVerified)
                .HasDefaultValue(false)
                .HasColumnName("is_verified");
            entity.Property(e => e.LastActiveTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_active_time");
            entity.Property(e => e.LastLoginTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login_time");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.LoginAttempt)
                .HasDefaultValue((short)0)
                .HasColumnName("login_attempt");
            entity.Property(e => e.NiNo)
                .HasMaxLength(60)
                .HasColumnName("ni_no");
            entity.Property(e => e.PassportExpirationDate).HasColumnName("passport_expiration_date");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(60)
                .HasColumnName("passport_number");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(60)
                .HasColumnName("password_hash");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.ProfilePicture).HasColumnName("profile_picture");
            entity.Property(e => e.RefreshTokens)
                .HasDefaultValueSql("ARRAY[]::text[]")
                .HasColumnName("refresh_tokens");
            entity.Property(e => e.Title)
                .HasMaxLength(20)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByOperator).HasColumnName("updated_by_operator");
            entity.Property(e => e.UpdatedByUser).HasColumnName("updated_by_user");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(10)
                .HasColumnName("verification_code");

            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_company_id_fkey");

            entity.HasOne(d => d.Permission).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("employee_permission_id_fkey");
        });

        modelBuilder.Entity<Hierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hierarchy_pkey");

            entity.ToTable("hierarchy");

            entity.HasIndex(e => new { e.ManagerId, e.SubordinateId }, "uq_hierarchies").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.SubordinateId).HasColumnName("subordinate_id");

            entity.HasOne(d => d.Manager).WithMany(p => p.HierarchyManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("fk_manager");

            entity.HasOne(d => d.Subordinate).WithMany(p => p.HierarchySubordinates)
                .HasForeignKey(d => d.SubordinateId)
                .HasConstraintName("fk_subordinate");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("note_pkey");

            entity.ToTable("note");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.NoteContent).HasColumnName("note_content");
            entity.Property(e => e.OperatorId).HasColumnName("operator_id");

            entity.HasOne(d => d.Operator).WithMany(p => p.Notes)
                .HasForeignKey(d => d.OperatorId)
                .HasConstraintName("note_operator_id_fkey");
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("operator_pkey");

            entity.ToTable("operator");

            entity.HasIndex(e => e.Email, "operator_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedBy).HasColumnName("added_by");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.IsTerminated)
                .HasDefaultValue(false)
                .HasColumnName("is_terminated");
            entity.Property(e => e.IsVerified)
                .HasDefaultValue(false)
                .HasColumnName("is_verified");
            entity.Property(e => e.LastActiveTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_active_time");
            entity.Property(e => e.LastLoginTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login_time");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.LoginAttempt)
                .HasDefaultValue((short)0)
                .HasColumnName("login_attempt");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(60)
                .HasColumnName("password_hash");
            entity.Property(e => e.ProfilePicture).HasColumnName("profile_picture");
            entity.Property(e => e.RefreshTokens)
                .HasDefaultValueSql("ARRAY[]::text[]")
                .HasColumnName("refresh_tokens");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedByOprtator).HasColumnName("updated_by_oprtator");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.VerficationCode)
                .HasMaxLength(10)
                .HasColumnName("verfication_code");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.HasIndex(e => new { e.PermissionName, e.CompanyId }, "permission_permission_name_company_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedBy).HasColumnName("added_by");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EnableAddEmployees)
                .HasDefaultValue(false)
                .HasColumnName("enable_add_employees");
            entity.Property(e => e.EnableAddLateness)
                .HasDefaultValue(false)
                .HasColumnName("enable_add_lateness");
            entity.Property(e => e.EnableAddManditoryLeave)
                .HasDefaultValue(false)
                .HasColumnName("enable_add_manditory_leave");
            entity.Property(e => e.EnableApproveAbsence)
                .HasDefaultValue(false)
                .HasColumnName("enable_approve_absence");
            entity.Property(e => e.EnableCreatePattern)
                .HasDefaultValue(false)
                .HasColumnName("enable_create_pattern");
            entity.Property(e => e.EnableCreateRotas)
                .HasDefaultValue(false)
                .HasColumnName("enable_create_rotas");
            entity.Property(e => e.EnableDeleteEmployee)
                .HasDefaultValue(false)
                .HasColumnName("enable_delete_employee");
            entity.Property(e => e.EnableTerminateEmployees)
                .HasDefaultValue(false)
                .HasColumnName("enable_terminate_employees");
            entity.Property(e => e.EnableViewEmployeeNotifications)
                .HasDefaultValue(false)
                .HasColumnName("enable_view_employee_notifications");
            entity.Property(e => e.EnableViewEmployeePayroll)
                .HasDefaultValue(false)
                .HasColumnName("enable_view_employee_payroll");
            entity.Property(e => e.EnableViewEmployeeSensitiveInformation)
                .HasDefaultValue(false)
                .HasColumnName("enable_view_employee_sensitive_information");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(60)
                .HasColumnName("permission_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Company).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("permission_company_id_fkey");
        });

        modelBuilder.Entity<WorkingPattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("working_pattern_pkey");

            entity.ToTable("working_pattern");

            entity.HasIndex(e => new { e.CompanyId, e.PatternName }, "unique_pattern_name_per_company").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddedBy).HasColumnName("added_by");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FridayEndTime).HasColumnName("friday_end_time");
            entity.Property(e => e.FridayStartTime).HasColumnName("friday_start_time");
            entity.Property(e => e.MondayEndTime).HasColumnName("monday_end_time");
            entity.Property(e => e.MondayStartTime).HasColumnName("monday_start_time");
            entity.Property(e => e.PatternName)
                .HasMaxLength(60)
                .HasColumnName("pattern_name");
            entity.Property(e => e.SaturdayEndTime).HasColumnName("saturday_end_time");
            entity.Property(e => e.SaturdayStartTime).HasColumnName("saturday_start_time");
            entity.Property(e => e.SundayEndTime).HasColumnName("sunday_end_time");
            entity.Property(e => e.SundayStartTime).HasColumnName("sunday_start_time");
            entity.Property(e => e.ThursdayEndTime).HasColumnName("thursday_end_time");
            entity.Property(e => e.ThursdayStartTime).HasColumnName("thursday_start_time");
            entity.Property(e => e.TuesdayEndTime).HasColumnName("tuesday_end_time");
            entity.Property(e => e.TuesdayStartTime).HasColumnName("tuesday_start_time");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.WednesdayEndTime).HasColumnName("wednesday_end_time");
            entity.Property(e => e.WednesdayStartTime).HasColumnName("wednesday_start_time");

            entity.HasOne(d => d.Company).WithMany(p => p.WorkingPatterns)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_company");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
