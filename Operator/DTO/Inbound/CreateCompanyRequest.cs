namespace OP.DTO.Inbound
{
    public class CreateCompanyRequest
    {
        public string CompanyName { get; set; } = null!;

        public string LicenceNumber { get; set; } = null!;

        public string AccountNumber { get; set; } = null!;

        public DateOnly AnnualLeaveStartDate { get; set; }

        public string? Lang { get; set; }

        public string? Country { get; set; }

        public bool IsSpecialClient { get; set; }

        public int MaxEmployeesAllowed { get; set; }

        public string? SecurityQuestionOne { get; set; }

        public string? SecurityQuestionTwo { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string PhoneNumber { get; set; } = null!;

    }
}
