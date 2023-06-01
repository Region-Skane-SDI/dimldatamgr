using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RS.SDI.DimlDataMgr.Server.Controllers
{
    [ApiController]
    [Route("api/v0/dataproduct")]
    public class DataProductController : ControllerBase
    {
        private readonly ILogger<DataProductController> _logger;

        public DataProductController(ILogger<DataProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{dimlid}/{environment}/availability")]
        public AvailabilityStatus GetAvailability(string dimlid, string environment)
        {
            // TODO: Return REAL status
            return AvailabilityStatus.Available;
        }

        [HttpPut]
        [Route("{dimlid}/{environment}/availability")]
        public IActionResult SetAvailability(string dimlid, string environment, AvailabilityStatus status)
        {
            // TODO: Update status
            return Ok();
        }

        [HttpGet]
        [Route("{dimlid}/{environment}/connectioninfo")]
        public DataProductConnectionInfo GetConnectionInfo(string dimlid, string environment)
        {
            // TODO: Return REAL info
            return new DataProductConnectionInfo { Dimlid = dimlid, SqlConnectionString = "Provider=xxx;Initial Catalog=xxx;Integrated Security=xxx" };
        }

        [HttpGet]
        [Route("{dimlid}/{environment}/generalinfo")]
        public DataProductGeneralInfo GetGeneralInfo(string dimlid, string environment)
        {
            // TODO: Return REAL info
            return new DataProductGeneralInfo { Dimlid = dimlid, ProductOwner = "99999@skane.se", InformationOwner = "99999@skane.se", Sla = SlaLevel.Silver };
        }
    }
}