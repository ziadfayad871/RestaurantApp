using Microsoft.AspNetCore.Mvc;
using RestaurantApp.API.Models;
using RestaurantApp.Application.Common;

namespace RestaurantApp.API.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleResult<T>(ServiceResult<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(APIResponse<T>.Success(result.Data!));
            }

            return HandleFailure(result);
        }

        protected IActionResult HandleFailure<T>(ServiceResult<T> result)
        {
            return StatusCode(
                (int)result.Status,
                APIResponse<object>.Failure((int)result.Status, result.Message, result.Errors.ToArray()));
        }
    }
}
