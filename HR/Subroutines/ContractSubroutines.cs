using Common;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using Models;

namespace HR.Subroutines
{
    public class ContractSubroutines
    {

        public static int CheckAndGetLeaveUnit(List<Contract> existingContracts, CreateContractDto? newContract)
        {
            // 0 no previous contracts
            // 1 days
            // 2 hours
            bool isDays;
            int leaveUnit = 0;

            if (existingContracts.Count > 0)
            {
                leaveUnit = existingContracts[0].IsDays == true ? 1 : 2;
                isDays = existingContracts[0].IsDays;
            }
            else if (newContract != null)
            {
                leaveUnit = newContract.IsDays == true ? 1 : 2;
                isDays = newContract.IsDays;
            }
            else
            {
                throw new Exception("Unexpected Error, attempted to calculate leave, but some required data was missing.");
            }

            for (int i = 1; i < existingContracts.Count; i++)
            {
                if (existingContracts[i].IsDays != isDays) throw new Exception("The contracts may be out of sync regarding the leave units, please contact the DeviseHR customer service.");
            }

            if (newContract != null)
            {
                if (newContract.IsDays != isDays) throw new Exception("You can not add a new contract with a different leave unit.");
            }

            return leaveUnit;
        }


        public static List<StartAndEndDate> GetLeaveYearCount(DateOnly requestedDate, DateOnly employeeAnnualLeaveStartDate, DateOnly firstContractStartDate)
        {
            // NOTE
            // ONLY GETS YEAR GETS LEAVE YEARS WHEN DATE IS LOCATED WITHIN AN ACTUAL LEAVE YEAR
            // IF leave year first starts on 01-04-2022 and you request 01-01-2022
            // IT WILL NOT RESPONSE WITH THE 2022 leave year as being accurate
            // YOU MUST SELECT LEAVE YEAR WITH THE START OF THE LEAVE YEAR IN DATES AND MONTHS TO BE ACCURATE
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly firstLeaveYearStartDate = DateModifier.GetLeaveYearStartDate(firstContractStartDate, employeeAnnualLeaveStartDate);
            DateOnly currentLeaveYearStartDate = DateModifier.GetLeaveYearStartDate(today, employeeAnnualLeaveStartDate);

            List<StartAndEndDate> leaveYearDates = new List<StartAndEndDate>();

            int leaveYearCount = (currentLeaveYearStartDate.Year + 2) - firstLeaveYearStartDate.Year;

            for (int i = 0; i < leaveYearCount; i++)
            {
                StartAndEndDate startAndEndDate = new StartAndEndDate();
                DateOnly leaveYearDate = firstLeaveYearStartDate.AddYears(i);
                startAndEndDate.StartDate = leaveYearDate;
                startAndEndDate.EndDate = leaveYearDate.AddYears(1).AddDays(-1);

                if (requestedDate >= startAndEndDate.StartDate && requestedDate <= startAndEndDate.EndDate) startAndEndDate.IsSelectedYear = true;

                leaveYearDates.Add(startAndEndDate);
            }
            //}

            return leaveYearDates;
        }

    }
}
