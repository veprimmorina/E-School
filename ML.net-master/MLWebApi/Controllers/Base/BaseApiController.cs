using Core.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace MLWebApi.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    /// <summary>
    ///     All controllers will inherit Execute to return their methods after passing a response to it
    /// </summary>
    /// <returns>A generic HTTP Ok response with the specified object as the response body</returns>
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

