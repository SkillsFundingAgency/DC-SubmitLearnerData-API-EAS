using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.Api.Common.Extensions;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.PublicApi.EAS.Constants;
using ESFA.DC.PublicApi.EAS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESFA.DC.PublicApi.EAS.Controllers
{
    [Authorize(Policy = PolicyNameConstants.EasAccess)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EasController : ControllerBase
    {
        private readonly IIndex<int, IEASRepository> _easRepositories;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EasController"/> class.
        /// </summary>
        public EasController(IIndex<int, IEASRepository> easRepositories, ILogger logger)
        {
            _easRepositories = easRepositories;
            _logger = logger;
        }

        /// <summary>
        /// Get EAS data for all providers or filter by provider
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="academicYear"></param>
        /// <param name="ukprn">Ukprn</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="pageNumber">PageNumber</param>
        /// <returns>List of EASSubmission dto with response header named "X-pagination" for paging information containing following
        ///  int TotalItems
        ///  int PageNumber
        ///  int PageSize
        ///  int TotalPages.
        /// </returns>
        [Route("{academicYear}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<List<Dtos.EasSubmission>>> Get(CancellationToken cancellationToken, int academicYear, [FromQuery] string ukprn = null, [FromQuery] int? pageSize = null, [FromQuery] int? pageNumber = null)
        {
            try
            {
                _easRepositories.TryGetValue(academicYear, out var repository);

                if (repository == null)
                {
                    return BadRequest("Academic year passed is not a valid value");
                }

                var data = await repository.GetSubmissionValues(cancellationToken, ukprn, pageSize ?? DefaultConstants.DefaultPageSize, pageNumber ?? DefaultConstants.DefaultPageNumber);

                if (data?.TotalItems > 0)
                {
                    Response.AddPaginationHeader(data);
                    _logger.LogDebug($"Call to Get for {academicYear} completed with data, count : {data?.TotalItems}");

                    return Ok(data.List);
                }

                _logger.LogDebug($"Call to Get with {academicYear} completed with data, no result");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured in getting 1819 eas data", e);
                return BadRequest("Error occured in getting data");
            }
        }
    }
}