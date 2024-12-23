namespace HR.DTO.outbound
{
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

    public class ContractAndLeaveYearCount
    {
        public ContractDto contract { get; set; } = new ContractDto();
        public List<StartAndEndDate> leaveYears { get; set; } = new List<StartAndEndDate>();
    }
}
