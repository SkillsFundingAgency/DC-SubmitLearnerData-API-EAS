using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESFA.DC.PublicApi.EAS.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Ok");
        }
    }
}