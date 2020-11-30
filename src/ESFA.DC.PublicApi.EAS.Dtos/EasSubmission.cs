using System;

namespace ESFA.DC.PublicApi.EAS.Dtos
{
    public class EasSubmission
    {
        public string Ukprn { get; set; }

        public Guid SubmissionId { get; set; }

        public long SubmissionCollectionPeriod { get; set; }

        public int PaymentType { get; set; }

        public string PaymentTypeName { get; set; }

        public decimal Amount { get; set; }
    }
}