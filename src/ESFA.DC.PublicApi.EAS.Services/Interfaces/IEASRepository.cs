using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Api.Common.Paging.Interfaces;
using ESFA.DC.PublicApi.EAS.Dtos;

namespace ESFA.DC.PublicApi.EAS.Services.Interfaces
{
    public interface IEASRepository
    {
        Task<IPaginatedResult<EasSubmission>> GetSubmissionValues(CancellationToken cancellationToken, string ukprn = null, int pageSize = 0, int pageNumber = 0);
    }
}
