using System.Net;
using Domain.Exceptions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case BusinessLogicException ex:
            {
                var errorContent = new { Errors = new { BusinessLogicError = ex.Message } };
                context.Result = new JsonResult(errorContent) { StatusCode = (int)HttpStatusCode.BadRequest };
                break;
            }
            case PermissionException ex:
            {
                var errorContent = new { Errors = new { PermissionError = ex.Message } };
                context.Result = new JsonResult(errorContent) { StatusCode = (int)HttpStatusCode.Forbidden };
                break;
            }
        }
    }
}