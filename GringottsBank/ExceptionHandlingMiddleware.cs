using GringottsBank.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GringottsBank
{
    public class ExceptionHandlingMiddleware
    {
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException exception)
            {
                await WriteErrorResponse(context, 400, exception.Errors);
            }
            catch(Exception exception)
            {
                await WriteErrorResponse(context, 500, new DataContracts.Error
                {
                    Code = Error.Code.ApplicationException,
                    Message = Error.Message.ApplicationException
                });
            }
        }
        private async Task WriteErrorResponse(HttpContext httpContext, int httpStatusCode, DataContracts.Error error)
        {
            httpContext.Response.StatusCode = httpStatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }

        private async Task WriteErrorResponse(HttpContext httpContext, int httpStatusCode, List<DataContracts.Error> errors)
        {
            httpContext.Response.StatusCode = httpStatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(errors));
        }
    }
}
