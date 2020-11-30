using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Paging.Interfaces;
using ESFA.DC.Api.Common.Paging.Pagination;
using ESFA.DC.EAS2021.EF.Interface;
using ESFA.DC.PublicApi.EAS.Services.Interfaces;

namespace ESFA.DC.PublicApi.EAS.Services
{
    public class Eas2021Repository : IEASRepository
    {
        private readonly IEasdbContext _eas2021DbContext;

        public Eas2021Repository(IEasdbContext eas2021DbContext)
        {
            _eas2021DbContext = eas2021DbContext;
        }

        public async Task<IPaginatedResult<Dtos.EasSubmission>> GetSubmissionValues(CancellationToken cancellationToken, string ukprn = null, int pageSize = 0, int pageNumber = 0)
        {
            IPaginatedResult<Dtos.EasSubmission> result;

            using (var context = _eas2021DbContext)
            {
                var data = context.EasSubmissionValues
                    .Where(x => x.Payment.Fm36);

                if (!string.IsNullOrEmpty(ukprn))
                {
                    data = data.Where(x => x.EasSubmission.Ukprn == ukprn);
                }

                var projectedData = data.OrderBy(x => x.EasSubmission.UpdatedOn)
                    .Select(value => new Dtos.EasSubmission()
                    {
                        Ukprn = value.EasSubmission.Ukprn,
                        Amount = value.PaymentValue,
                        SubmissionId = value.SubmissionId,
                        PaymentTypeName = value.Payment.PaymentName,
                        PaymentType = value.PaymentId,
                        SubmissionCollectionPeriod = value.CollectionPeriod,
                    });

                result = await PaginatedResultFactory<Dtos.EasSubmission>.CreatePaginatedResultAsync(projectedData, pageSize, pageNumber, cancellationToken);

                return result;
            }
        }
    }
}