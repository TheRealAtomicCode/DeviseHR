using HR.DTO.Inbound;
using Models;

namespace HR.Subroutines
{
    public class ContractVerification
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

    }
}
