using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Zupan.CodeReview.WebApi.Exceptions;
using Zupan.CodeReview.WebApi.Models;

namespace Zupan.CodeReview.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;

            if (context.Exception is ApiException ex)
            {
                context.Exception = null;
                apiError = new ApiError(ex.Message) { Errors = ex.Errors };

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
            }
            else
            {
                var msg = context.Exception.GetBaseException().Message;
                var stack = context.Exception.StackTrace;

                apiError = new ApiError(msg)
                {
                    Detail = stack
                };

                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
