using Models;

namespace HR.DTO
{
    public partial class WorkingPatternRequest
    {
        public string PatternName { get; set; } = null!;
        public TimeOnly? MondayStartTime { get; set; }
        public TimeOnly? MondayEndTime { get; set; }
        public TimeOnly? TuesdayStartTime { get; set; }
        public TimeOnly? TuesdayEndTime { get; set; }
        public TimeOnly? WednesdayStartTime { get; set; }
        public TimeOnly? WednesdayEndTime { get; set; }
        public TimeOnly? ThursdayStartTime { get; set; }
        public TimeOnly? ThursdayEndTime { get; set; }
        public TimeOnly? FridayStartTime { get; set; }
        public TimeOnly? FridayEndTime { get; set; }
        public TimeOnly? SaturdayStartTime { get; set; }
        public TimeOnly? SaturdayEndTime { get; set; }
        public TimeOnly? SundayStartTime { get; set; }
        public TimeOnly? SundayEndTime { get; set; }
    }


}
