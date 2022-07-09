using System;
using System.Net;
using System.Threading.Tasks;
using FeedbackSchool.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FeedbackSchool.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    private readonly RequestDelegate _requestDelegate;

    public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlingMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context,
                e.Message,
                HttpStatusCode.InternalServerError,
                "Произошла неизвестная ошибка. \nИнформация о ней уже была отправлена разработчикам");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode,
                                            string message)
    {
        _logger.LogError(exMsg);

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int) httpStatusCode;

        var errorDto = new ErrorDto
        {
            StatusCode = (int) httpStatusCode,
            Message = message
        };

        await response.WriteAsJsonAsync(errorDto.ToString());
    }
}