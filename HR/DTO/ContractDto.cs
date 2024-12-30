using Models;

namespace HR.DTO
{

    public class AddContractRequest
    {
        public int EmployeeId { get; set; }
        public int? PatternId { get; set; } = null;
        public int ContractType { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public bool IsDays { get; set; }
        public int ContractedHoursPerWeek { get; set; }
        public int ContractedDaysPerWeek { get; set; }
        public int CompanyHoursPerWeek { get; set; }
        public int CompanyDaysPerWeek { get; set; }
        public int AverageWorkingDay { get; set; }
        public int CompanyLeaveEntitlement { get; set; }
        public int ContractedLeaveEntitlement { get; set; }
        public int FirstLeaveAllowence { get; set; }
        public int NextLeaveAllowence { get; set; }
        public int TermTimeId { get; set; }
        public int DiscardedId { get; set; }
    }

    public class NewContractCalculationResult
    {
        public NewContractCalculationResult(double _contractleaveEntitlementPerYear, double _contractEntitlement)
        {
            ContractleaveEntitlementPerYear = _contractleaveEntitlementPerYear;
            ThisContractEntitlement = _contractEntitlement;
        }
        public double ContractleaveEntitlementPerYear { get; set; }
        public double ThisContractEntitlement { get; set; }
    }

    public partial class ContractDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int? PatternId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AddedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int ContractType { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public int ContractedHoursPerWeek { get; set; }
        public int CompanyHoursPerWeek { get; set; }
        public int ContractedDaysPerWeek { get; set; }
        public int CompanyDaysPerWeek { get; set; }
        public int AverageWorkingDay { get; set; }
        public bool IsDays { get; set; }
        public int CompanyLeaveEntitlement { get; set; }
        public int ContractedLeaveEntitlement { get; set; }
        public int FirstLeaveAllowence { get; set; }
        public int NextLeaveAllowence { get; set; }
        public int? TermTimeId { get; set; }
        public int? DiscardedId { get; set; }
    }


    public class LeaveYear
    {
        public DateOnly LeaveYearStartDate { get; set; }
        public int AnnualLeaveEntitlement { get; set; }
    }

    public class StartAndEndDate
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsSelectedYear { get; set; } = false;
    }

    public class LeaveYearResponse
    {
        public List<VirtualContract> leaveYearContracts { get; set; } = new List<VirtualContract>();
        public List<AbsenceDto> absences { get; set; } = new List<AbsenceDto>();
        public List<StartAndEndDate> leaveYears { get; set; } = new List<StartAndEndDate>();
    }


    public class VirtualContract
    {
        private VirtualContract virtualContract;

        public VirtualContract()
        {
        }

        public VirtualContract(VirtualContract virtualContract)
        {
            this.virtualContract = virtualContract;
        }


        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int? PatternId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AddedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int ContractType { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public int ContractedHoursPerWeek { get; set; }
        public int CompanyHoursPerWeek { get; set; }
        public int ContractedDaysPerWeek { get; set; }
        public int CompanyDaysPerWeek { get; set; }
        public int AverageWorkingDay { get; set; }
        public bool IsDays { get; set; }
        public int CompanyLeaveEntitlement { get; set; }
        public int ContractedLeaveEntitlement { get; set; }
        public int FirstLeaveAllowence { get; set; }
        public int NextLeaveAllowence { get; set; }
        public int? TermTimeId { get; set; }
        public int? DiscardedId { get; set; }

        // difference from contract dto
        public DateOnly? ContractEndDate { get; set; } = null;
        public int Allowance { get; set; }
    }

}
