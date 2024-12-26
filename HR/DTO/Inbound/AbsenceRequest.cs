namespace HR.DTO.Inbound
{
    public class AddAbsenceRequest
    {
        public int EmployeeId { get; set; }
        public DateOnly AbsenceStartDate { get; set; }
        public DateOnly AbsenceEndDate { get; set; }
        public bool? IsFirstHalfDay { get; set; }
        public bool IsDays { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int? DaysDeducted { get; set; }
        public int? HoursDeducted { get; set; }
        public int AbsenceTypeId { get; set; }
        public string? Comment { get; set; }
    }


}
