using HR.DTO;

namespace HR.Subroutines
{
    public class WorkingPatternSubroutines
    {

        public static (int workingDays, int workingHours) ExtractWorkingDaysAndHours(WorkingPatternDto pattern)
        {
            int workingDays = 0;
            int totalWorkingMinutes = 0;

            // Helper method to calculate working minutes for a given start and end time
            int GetWorkingMinutes(TimeOnly? startTime, TimeOnly? endTime)
            {
                if (startTime.HasValue && endTime.HasValue)
                {
                    // Calculate the difference between start and end times
                    var start = startTime.Value;
                    var end = endTime.Value;

                    if (end < start) // If end time is before start time, add 24 hours
                        end = end.AddHours(24);

                    return (int)(end.ToTimeSpan() - start.ToTimeSpan()).TotalMinutes;
                }
                return 0;
            }

            // Monday
            totalWorkingMinutes += GetWorkingMinutes(pattern.MondayStartTime, pattern.MondayEndTime);
            if (pattern.MondayStartTime.HasValue && pattern.MondayEndTime.HasValue) workingDays++;

            // Tuesday
            totalWorkingMinutes += GetWorkingMinutes(pattern.TuesdayStartTime, pattern.TuesdayEndTime);
            if (pattern.TuesdayStartTime.HasValue && pattern.TuesdayEndTime.HasValue) workingDays++;

            // Wednesday
            totalWorkingMinutes += GetWorkingMinutes(pattern.WednesdayStartTime, pattern.WednesdayEndTime);
            if (pattern.WednesdayStartTime.HasValue && pattern.WednesdayEndTime.HasValue) workingDays++;

            // Thursday
            totalWorkingMinutes += GetWorkingMinutes(pattern.ThursdayStartTime, pattern.ThursdayEndTime);
            if (pattern.ThursdayStartTime.HasValue && pattern.ThursdayEndTime.HasValue) workingDays++;

            // Friday
            totalWorkingMinutes += GetWorkingMinutes(pattern.FridayStartTime, pattern.FridayEndTime);
            if (pattern.FridayStartTime.HasValue && pattern.FridayEndTime.HasValue) workingDays++;

            // Saturday
            totalWorkingMinutes += GetWorkingMinutes(pattern.SaturdayStartTime, pattern.SaturdayEndTime);
            if (pattern.SaturdayStartTime.HasValue && pattern.SaturdayEndTime.HasValue) workingDays++;

            // Sunday
            totalWorkingMinutes += GetWorkingMinutes(pattern.SundayStartTime, pattern.SundayEndTime);
            if (pattern.SundayStartTime.HasValue && pattern.SundayEndTime.HasValue) workingDays++;

            // Calculate total working hours
            int workingHours = totalWorkingMinutes / 60;

            return (workingDays, workingHours);
        }
    }


}

