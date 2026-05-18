using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using ProvaMedGroup.DomainModel.Exceptions;

namespace ProvaMedGroup.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            object response;

            if (exception is TratedExceptions treatedException)
            {
                code = HttpStatusCode.BadRequest;
                response = new
                {
                    success = false,
                    statusCode = (int)code,
                    title = "Requisição inválida",
                    message = treatedException.Message
                };
            }
            else
            {
                if (_env.IsDevelopment())
                {
                    response = new
                    {
                        success = false,
                        statusCode = (int)code,
                        title = "Erro interno no servidor (DEV)",
                        message = exception.Message,
                        stackTrace = exception.StackTrace,
                        innerException = exception.InnerException?.Message
                    };
                }
                else
                {
                    response = new
                    {
                        success = false,
                        statusCode = (int)code,
                        title = "Erro interno no servidor",
                        message = "Ocorreu um erro inesperado. Tente novamente mais tarde."
                    };
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}