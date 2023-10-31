using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Exceptions;

namespace WhatIBoughtAPI.Middlewares;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private sealed record ErrorResponse(
        [property: JsonPropertyName("message")]
        string Message, 
        [property: JsonPropertyName("errors")]
        List<string> Errors
    );
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException or InvalidInputException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
        var error = exception switch
        {
            InvalidInputException invalidInputException => new ErrorResponse(invalidInputException.Message, invalidInputException.Errors),
            _ => new ErrorResponse(exception?.Message ?? "Erro interno do servidor", new List<string>())
        };
        var jsonError = JsonSerializer.Serialize(error);
        await context.Response.WriteAsync(jsonError);
    }
}