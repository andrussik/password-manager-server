using System.Net;
using Domain.Exceptions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Models;

namespace WebApp.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BusinessLogicException ex)
        {
            var errorContent = new ErrorContent { ErrorMessages = new List<string> { ex.Message } };
            context.Result = new JsonResult(errorContent) { StatusCode = (int)HttpStatusCode.BadRequest };
        }
    }
}