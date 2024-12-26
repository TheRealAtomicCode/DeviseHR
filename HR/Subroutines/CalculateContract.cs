using Common;
using HR.DTO;
using Mapster;
using Models;

namespace HR.Subroutines
{
    public class Calculate
    {

        public static List<VirtualContract> PlaceContractsInYear(List<VirtualContract> virtualContracts, DateOnly annualLeaveStartDate)
        {
            var leaveYearContracts = new List<VirtualContract>();

            var annualLeaveEndDate = annualLeaveStartDate.AddYears(1).AddDays(-1);

            if (virtualContracts.Count > 0)
            {

                for (int i = 0; i < virtualContracts.Count; i++)
                {
                    if(i == virtualContracts.Count - 1)
                    {
                        if (virtualContracts[i].ContractEndDate == null)
                        {
                            virtualContracts[i].ContractEndDate = annualLeaveStartDate.AddYears(1).AddDays(-1);
                        }
                    }

                    var newVC = virtualContracts[i].Adapt<VirtualContract>();

                    // contract starts before leave year and ends before leave year
                    if(virtualContracts[i].ContractStartDate <= annualLeaveStartDate && virtualContracts[i].ContractEndDate <= annualLeaveEndDate)
                    {
                        // take next allowance
                        newVC.ContractStartDate = annualLeaveStartDate;
                        newVC.ContractEndDate = virtualContracts[i].ContractEndDate;
                        newVC.Allowance = virtualContracts[i].NextLeaveAllowence;
                    }

                    // contract starts after leave year and ends before leave year
                    if (virtualContracts[i].ContractStartDate >= annualLeaveStartDate && virtualContracts[i].ContractEndDate <= annualLeaveEndDate)
                    {
                        // take first allowance
                        newVC.ContractStartDate = virtualContracts[i].ContractStartDate;
                        newVC.ContractEndDate = virtualContracts[i].ContractEndDate;
                        newVC.Allowance = virtualContracts[i].FirstLeaveAllowence;
                    }

                    // contract starts before leave year and ends after leave year
                    if (virtualContracts[i].ContractStartDate <= annualLeaveStartDate && virtualContracts[i].ContractEndDate >= annualLeaveEndDate)
                    {
                        // taske next allowance
                        newVC.ContractStartDate = annualLeaveStartDate;
                        newVC.ContractEndDate = annualLeaveEndDate;
                        newVC.Allowance = virtualContracts[i].NextLeaveAllowence;
                    }

                    // contract starts after leave year and ends after leave year
                    if (virtualContracts[i].ContractStartDate >= annualLeaveStartDate && virtualContracts[i].ContractEndDate >= annualLeaveEndDate)
                    {
                        // take first allowance
                        newVC.ContractStartDate = virtualContracts[i].ContractStartDate;
                        newVC.ContractEndDate = annualLeaveEndDate;
                        newVC.Allowance = virtualContracts[i].FirstLeaveAllowence;
                    }


                    leaveYearContracts.Add(newVC);
                }

            }

            return leaveYearContracts;
        }


        public static int CalculateLeaveYearEntitlementByDates(List<VirtualContract> virtualContracts, DateOnly leaveYearStartDate, DateOnly leaveYearEndDate, int leaveUnit)
        {
            int result = 0;

            if (virtualContracts.Count > 0)
            {
                double leaveYearEntitlement = 0;

                for (int i = 0; i < virtualContracts.Count; i++)
                {
                    int daysBetween = DateModifier.GetDaysBetween(leaveYearStartDate, leaveYearEndDate);

                    double contractLeaveRatio;

                    if (leaveUnit == 1)
                    {
                        contractLeaveRatio = (double)virtualContracts[i].ContractedDaysPerWeek / (double)virtualContracts[i].CompanyDaysPerWeek;
                    }
                    else if (leaveUnit == 2)
                    {
                        contractLeaveRatio = (double)virtualContracts[i].ContractedHoursPerWeek / (double)virtualContracts[i].CompanyHoursPerWeek;
                    }
                    else
                    {
                        throw new Exception("Unable to calculate leave, due to incorrect leave units");
                    }

                    double contractleaveEntitlementPerYear = contractLeaveRatio * virtualContracts[i].CompanyLeaveEntitlement;

                    int numberOfDatsInYear = DateModifier.GetNumberOfDaysInYear(leaveYearStartDate);

                    double thisContractEntitlement = (contractleaveEntitlementPerYear / numberOfDatsInYear) * daysBetween;

                    leaveYearEntitlement += thisContractEntitlement;
                }

                result = (int)Math.Ceiling(leaveYearEntitlement);
            }

            return result;
        }

        public static int CalculateLeaveYearEntitlementByContracts(List<VirtualContract> virtualContracts, int leaveUnit)
        {
            int result = 0;
            DateOnly leaveYearStartDate;
            DateOnly leaveYearEndDate;

            if (virtualContracts.Count > 0)
            {
                leaveYearStartDate = virtualContracts[0].ContractStartDate;

                if(virtualContracts[virtualContracts.Count - 1].ContractEndDate != null)
                {
                    leaveYearEndDate = (DateOnly)virtualContracts[virtualContracts.Count - 1].ContractEndDate!;
                }
                else
                {
                    leaveYearEndDate = virtualContracts[0].ContractStartDate.AddYears(1).AddDays(-1);
                }
                

                double leaveYearEntitlement = 0;

                for (int i = 0; i < virtualContracts.Count; i++)
                {
                    int daysBetween = DateModifier.GetDaysBetween(leaveYearStartDate, leaveYearEndDate);

                    double contractLeaveRatio;

                    if (leaveUnit == 1)
                    {
                        contractLeaveRatio = (double)virtualContracts[i].ContractedDaysPerWeek / (double)virtualContracts[i].CompanyDaysPerWeek;
                    }
                    else if (leaveUnit == 2)
                    {
                        contractLeaveRatio = (double)virtualContracts[i].ContractedHoursPerWeek / (double)virtualContracts[i].CompanyHoursPerWeek;
                    }
                    else
                    {
                        throw new Exception("Unable to calculate leave, due to incorrect leave units");
                    }

                    double contractleaveEntitlementPerYear = contractLeaveRatio * virtualContracts[i].CompanyLeaveEntitlement;

                    int numberOfDatsInYear = DateModifier.GetNumberOfDaysInYear(leaveYearStartDate);

                    double thisContractEntitlement = (contractleaveEntitlementPerYear / numberOfDatsInYear) * daysBetween;

                    leaveYearEntitlement += thisContractEntitlement;
                }

                result = (int)Math.Ceiling(leaveYearEntitlement);
            }

            return result;
        }


        //public static int CalculateLeaveYearEntitlement(List<VirtualContract> virtualContracts, DateOnly annualLeaveStartDate, DateOnly newContractStartDate, int leaveUnit)
        //{
        //    int result = 0;

        //    if (virtualContracts.Count > 0)
        //    {
        //        double leaveYearEntitlement = 0;

        //        for (int i = 0; i < virtualContracts.Count; i++)
        //        {
        //            DateOnly contractStartDate;
        //            DateOnly contractEndDate;

        //            // get contract start date
        //            if (virtualContracts[i].ContractStartDate < annualLeaveStartDate)
        //            {
        //                contractStartDate = annualLeaveStartDate;
        //            }
        //            else
        //            {
        //                contractStartDate = virtualContracts[i].ContractStartDate;
        //            }

        //            if (i == 1 - virtualContracts.Count)
        //            {
        //                contractEndDate = newContractStartDate;
        //            }
        //            else
        //            {
        //                if(i + 1 < virtualContracts.Count)
        //                {
        //                    contractEndDate = virtualContracts[i + 1].ContractStartDate;
        //                }
        //                else
        //                {
        //                    contractEndDate = newContractStartDate.AddDays(-1);
        //                }

        //            }

        //            int daysBetween = DateModifier.GetDaysBetween(contractStartDate, contractEndDate);

        //            double contractLeaveRatio;

        //            if (leaveUnit == 1)
        //            {
        //                contractLeaveRatio = (double)virtualContracts[i].ContractedDaysPerWeek / (double)virtualContracts[i].CompanyDaysPerWeek;
        //            }
        //            else if(leaveUnit == 2)
        //            {
        //                contractLeaveRatio = (double)virtualContracts[i].ContractedHoursPerWeek / (double)virtualContracts[i].CompanyHoursPerWeek;
        //            }
        //            else
        //            {
        //                throw new Exception("Unable to calculate leave, due to incorrect leave units");
        //            }

        //            double contractleaveEntitlementPerYear = contractLeaveRatio * virtualContracts[i].CompanyLeaveEntitlement;

        //            int numberOfDatsInYear = DateModifier.GetNumberOfDaysInYear(annualLeaveStartDate);

        //            double thisContractEntitlement = (contractleaveEntitlementPerYear / numberOfDatsInYear) * daysBetween;

        //            leaveYearEntitlement += thisContractEntitlement;
        //        }

        //        result = (int)Math.Ceiling(leaveYearEntitlement);
        //    }

        //    return result;
        //}


        //public static NewContractCalculationResult CalculateNewContractEntitlementMut(CreateContractDto newContract, DateOnly contractStartDate, DateOnly annualLeaveStartDate, bool isDays)
        //{
        //    // get contract start date
        //    if (contractStartDate < annualLeaveStartDate) throw new Exception("Unexpected error occured while calculating the current contract");


        //    int daysBetween = DateModifier.GetDaysBetween(contractStartDate, annualLeaveStartDate.AddYears(1).AddDays(-1));

        //    double contractLeaveRatio;

        //    if (isDays)
        //    {
        //        contractLeaveRatio = (double)newContract.ContractedDaysPerWeek / (double)newContract.CompanyDaysPerWeek;
        //    }
        //    else
        //    {
        //        contractLeaveRatio = (double)newContract.ContractedHoursPerWeek / (double)newContract.CompanyHoursPerWeek;
        //    }

        //    double contractleaveEntitlementPerYear = contractLeaveRatio * newContract.CompanyLeaveEntitlement;

        //    double thisContractEntitlement = (contractleaveEntitlementPerYear / 365) * daysBetween;

        //    NewContractCalculationResult newContractCalculationResult = new NewContractCalculationResult(contractleaveEntitlementPerYear, thisContractEntitlement);

        //    return newContractCalculationResult;
        //}

        public static NewContractCalculationResult CalculateNewContractEntitlementMut(AddContractRequest newContract, DateOnly annualLeaveStartDate, int leaveUnit)
        {
            // get contract start date
            if (newContract.ContractStartDate < annualLeaveStartDate)
            {
                newContract.ContractStartDate = newContract.ContractStartDate.AddYears(1);
            }

            int daysBetween = DateModifier.GetDaysBetween(newContract.ContractStartDate, annualLeaveStartDate.AddYears(1).AddDays(-1));

            double contractLeaveRatio;

            if (leaveUnit == 1)
            {
                contractLeaveRatio = (double)newContract.ContractedDaysPerWeek / (double)newContract.CompanyDaysPerWeek;
            }
            else if(leaveUnit == 2)
            {
                contractLeaveRatio = (double)newContract.ContractedHoursPerWeek / (double)newContract.CompanyHoursPerWeek;
            }
            else
            {
                throw new Exception("Unable to calculate leave, due to incorrect leave units");
            }

            double contractleaveEntitlementPerYear = contractLeaveRatio * newContract.CompanyLeaveEntitlement;

            int numberOfDaysInYear = DateModifier.GetNumberOfDaysInYear(annualLeaveStartDate);

            double thisContractEntitlement = (contractleaveEntitlementPerYear / numberOfDaysInYear) * daysBetween;

            NewContractCalculationResult newContractCalculationResult = new NewContractCalculationResult(contractleaveEntitlementPerYear, thisContractEntitlement);

            return newContractCalculationResult;
        }


    }
}
