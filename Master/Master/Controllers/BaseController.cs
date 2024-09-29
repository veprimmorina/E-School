using Microsoft.AspNetCore.Mvc;
using Master.Core.Wrappers;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateResponse<T>(BaseResponse<T> response)
        {
            if (response is null)
            {
                return NotFound();
            }

            if (response.StatusCode == (int)Core.Wrappers.StatusCodes.NotFound)
            {
                return NotFound(response);
            }

            if (response.StatusCode == (int)Core.Wrappers.StatusCodes.BadRequest)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
