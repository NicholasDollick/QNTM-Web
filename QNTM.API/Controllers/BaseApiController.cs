using Microsoft.AspNetCore.Mvc;
using QNTM.API.Helpers;

namespace QNTM.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}