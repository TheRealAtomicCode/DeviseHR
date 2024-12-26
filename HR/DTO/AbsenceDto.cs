using Models;

namespace HR.DTO
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

    public partial class AbsenceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int ContractId { get; set; }
        public DateOnly AbsenceStartDate { get; set; }
        public DateOnly AbsenceEndDate { get; set; }
        public bool? IsFirstHalfDay { get; set; }
        public bool IsDays { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int? DaysDeducted { get; set; }
        public int? HoursDeducted { get; set; }
        public int AbsenceTypes { get; set; }
        public string? Comments { get; set; }
        public int? ApprovedId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsPending { get; set; }
        public int? ApprovedByAdmin { get; set; }
        public int AddedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual AbsenceType AbsenceTypesNavigation { get; set; } = null!;
    }


}
