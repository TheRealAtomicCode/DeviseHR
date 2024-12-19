namespace HR.DTO.Inbound
{
    public class CreateContractDto
    {
        public int EmployeeId { get; set; }
        public int PatternId { get; set; }
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

}
