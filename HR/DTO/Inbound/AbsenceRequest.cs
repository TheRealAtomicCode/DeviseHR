namespace HR.DTO.Inbound
{
    public class AddAbsenceRequest
    {
        public int UserId { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int TimeDeducted { get; set; }
        public int AbsenceType { get; set; }
        public bool IsDays { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

}
