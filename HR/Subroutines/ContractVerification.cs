using HR.DTO.Inbound;
using Models;

namespace HR.Subroutines
{
    public class ContractVerification
    {

        public static bool CheckAllContractsHaveSameLeaveUnit(List<Contract> existingContracts, CreateContractDto? newContract)
        {
            bool isDays;

            if (existingContracts.Count > 0)
            {
                isDays = existingContracts[0].IsDays;
            }
            else if (newContract != null)
            {
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

            return isDays;
        }

    }
}
