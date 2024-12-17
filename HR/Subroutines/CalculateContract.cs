using Common;
using HR.DTO;
using HR.DTO.Inbound;
using Models;

namespace HR.Subroutines
{
    public class CalculateContract
    {

        public static int CalculateLeaveYearEntitlement(List<Contract> contracts, DateOnly annualLeaveStartDate, DateOnly newContractStartDate, bool isDays)
        {
            int result = 0;

            if (contracts.Count > 0)
            {
                double leaveYearEntitlement = 0;

                for (int i = 0; i < contracts.Count; i++)
                {
                    DateOnly contractStartDate;
                    DateOnly contractEndDate;

                    // get contract start date
                    if (contracts[i].ContractStartDate < annualLeaveStartDate)
                    {
                        contractStartDate = annualLeaveStartDate;
                    }
                    else
                    {
                        contractStartDate = contracts[i].ContractStartDate;
                    }

                    if (i == 1 - contracts.Count)
                    {
                        contractEndDate = newContractStartDate;
                    }
                    else
                    {
                        contractEndDate = contracts[i + 1].ContractStartDate;
                    }

                    int daysBetween = DateModifier.GetDaysBetween(contractStartDate, contractEndDate);

                    double contractLeaveRatio;

                    if (isDays)
                    {
                        contractLeaveRatio = (double)contracts[i].ContractedDaysPerWeek / (double)contracts[i].CompanyDaysPerWeek;
                    }
                    else
                    {
                        contractLeaveRatio = (double)contracts[i].ContractedHoursPerWeek / (double)contracts[i].CompanyHoursPerWeek;
                    }

                    double contractleaveEntitlementPerYear = contractLeaveRatio * contracts[i].CompanyLeaveEntitlement;

                    double thisContractEntitlement = (contractleaveEntitlementPerYear / 365) * daysBetween;

                    leaveYearEntitlement += thisContractEntitlement;
                }

                result = (int)Math.Ceiling(leaveYearEntitlement);
            }

            return result;
        }


        public static NewContractCalculationResult CalculateNewContractEntitlementMut(CreateContractDto newContract, DateOnly contractStartDate, DateOnly annualLeaveStartDate, bool isDays)
        {
            // get contract start date
            if (contractStartDate < annualLeaveStartDate) throw new Exception("Unexpected error occured while calculating the current contract");


            int daysBetween = DateModifier.GetDaysBetween(contractStartDate, annualLeaveStartDate.AddYears(1).AddDays(-1));

            double contractLeaveRatio;

            if (isDays)
            {
                contractLeaveRatio = (double)newContract.ContractedDaysPerWeek / (double)newContract.CompanyDaysPerWeek;
            }
            else
            {
                contractLeaveRatio = (double)newContract.ContractedHoursPerWeek / (double)newContract.CompanyHoursPerWeek;
            }

            double contractleaveEntitlementPerYear = contractLeaveRatio * newContract.CompanyLeaveEntitlement;

            double thisContractEntitlement = (contractleaveEntitlementPerYear / 365) * daysBetween;

            NewContractCalculationResult newContractCalculationResult = new NewContractCalculationResult(contractleaveEntitlementPerYear, thisContractEntitlement);

            return newContractCalculationResult;
        }


    }
}
