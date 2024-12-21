using Models;

namespace HR.DTO
{

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


}
