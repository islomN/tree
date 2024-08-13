using System.Net;
using Api.Helpers;
using Domain.Models;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Microsoft.IO;
using Newtonsoft.Json;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogService logService)
{
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var requestId = TimestampHelper.GetTimestamp();
            var log = "REQUEST ID: " + requestId +"\r\n";
            log += "DATETIME: " + DateTime.UtcNow.ToString("O") + "\r\n";
            log += await GetRequestInfo(context) + "\r\n";
            log += "MESSAGE:" + ex.Message + "\r\n";
            log += "STACKTRACE:" + ex.StackTrace;

            await logService.Save(requestId, log, context.RequestAborted);

            ErrorResponse errorResponse;
            if (ex is SecureException)
            {
                errorResponse = new ErrorResponse(
                    "Secure",
                    requestId,
                    new ErrorDataModel(ex.Message));
            }
            else
            {
                errorResponse = new ErrorResponse(
                    "Exception",
                    requestId,
                    new ErrorDataModel($"Internal server error ID = {requestId}"));
            }

            await HandleExceptionAsync(context, errorResponse);
        }
    }

    private async Task<string> GetRequestInfo(HttpContext context)
    {
        context.Request.EnableBuffering();

        await using var requestStream = _recyclableMemoryStreamManager.GetStream();
        await context.Request.Body.CopyToAsync(requestStream);
        var request = $"QUERYSTRING: {context.Request.QueryString} \r\n" +
                           $"REQUEST BODY: {ReadStreamInChunks(requestStream)
                               .Replace("\r\n", "")
                               .Replace("\r", "")
                               .Replace("\n", "")} ";
        context.Request.Body.Position = 0;

        return request;
    }

    private static Task HandleExceptionAsync(HttpContext context, ErrorResponse errorResponse)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }
    
    private static String ReadStreamInChunks(Stream stream)
    {
        const Int32 READ_CHUNK_BUFFER_LENGTH = 4096;

        stream.Seek(0, SeekOrigin.Begin);

        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);

        var readChunk = new Char[READ_CHUNK_BUFFER_LENGTH];
        Int32 readChunkLength;

        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, READ_CHUNK_BUFFER_LENGTH);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);

        return textWriter.ToString();
    }
}