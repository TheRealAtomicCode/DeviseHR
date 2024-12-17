namespace HR.DTO
{

    public class NewContractCalculationResult
    {
        public NewContractCalculationResult(double _contractleaveEntitlementPerYear, double _contractEntitlement)
        {
            ContractleaveEntitlementPerYear = _contractleaveEntitlementPerYear;
            ThisContractEntitlement = _contractEntitlement;
        }
        public double ContractleaveEntitlementPerYear { get; set; }
        public double ThisContractEntitlement { get; set; }
    }
}
