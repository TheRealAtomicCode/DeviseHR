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


        public static List<StartAndEndDate> GetLeaveYearCount(DateOnly annualLeaveStartDate, DateOnly providedDate)
        {
            // get leave year count if the provided date is for this year
            // not getting leave year counts if other year date is provided, because by default the user will get this year, but after
            // may request another year, which in that case, the user will already have the leave year count locally
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly leaveYearStartDate = DateModifier.GetLeaveYearStartDate(today, annualLeaveStartDate);

            List<StartAndEndDate> leaveYearDates = new List<StartAndEndDate>();
            if (providedDate >= leaveYearStartDate && providedDate < leaveYearStartDate.AddYears(1))
            {
                int leaveYearCount = providedDate.Year - leaveYearStartDate.Year + 2;
                for (int i = 0; i < leaveYearCount; i++)
                {
                    StartAndEndDate startAndEndDate = new StartAndEndDate();
                    DateOnly leaveYearDate = leaveYearStartDate.AddYears(i);
                    startAndEndDate.StartDate = leaveYearDate;
                    startAndEndDate.EndDate = leaveYearDate.AddYears(1).AddDays(-1);
                    leaveYearDates.Add(startAndEndDate);
                }
            }

            return leaveYearDates;
        }

    }
}
