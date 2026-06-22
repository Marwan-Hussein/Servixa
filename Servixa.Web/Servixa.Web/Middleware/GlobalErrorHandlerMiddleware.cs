using Servixa.Domain.Exceptions;
using Servixa.Shared.Commen.Responses;
using System.Text.Json;

namespace Servixa.Web.Middleware
{
    public class GlobalErrorHandlerMiddleware
        // nextMw is the next middleware in the pipeline, logger is used for logging exceptions.
        // env is used to determine the current hosting environment (development, production, etc.)
        // to provide more detailed error information in development mode.
        (RequestDelegate nextMW, ILogger<GlobalErrorHandlerMiddleware> logger, IWebHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await nextMW(context);
                // sometime the server can start sending the header of response to the client before we check the status code,
                // so we need to check if the response has started or not before we try to modify it.
                // because if the response has already started, we can't modify the status code or the content type, and we can't send a new response body.
                // check has started is used to ensure that server hasn't already sent a response to the client, because if we have, we can't modify the response anymore.
                if(context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    await HandleResponseAsync(context, 404, "The requested resource was not found.");
                }

            }   
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error : {ex.Message}");
                await HandelExceptionAsync(context, ex);
            }
        }

        public async Task HandleResponseAsync(HttpContext context,int statusCode ,string msg)
        { 
            context.Response.ContentType = "application/json"; // change from html to json because we want to return a json response to the client

            context.Response.StatusCode = statusCode; 

            var response = new ApiResponse<string>(msg,statusCode);  // wrapper design pattern to ensure consistent response structure across the application, even for error responses. 
            
            // convert from object ApiJson to json string to send it to the client,
            // and we use camelCase naming policy to ensure that the property names in the JSON response are in camelCase format,
            // which is a common convention in JavaScript and JSON data.

            var json = JsonSerializer.Serialize(response ,new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json);
        }

        public async Task HandelExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json"; // change from html to json because we want to return a json response to the client

            string detailedMessage = ex.InnerException != null ? $"{ex.Message} \n Inner Details: {ex.GetBaseException().Message}": ex.Message;
           
            // used if server error exception only
            string serverErrorMessage = env.IsDevelopment()? $"{detailedMessage} \n\nStackTrace:\n{ex.StackTrace}"
                                                            : "An unexpected error occurred. Please try again later.";

            var response = ex switch
            {
                NotFoundExceptionCustome => new ApiResponse<string>(ex.Message, 404),
                UnauthorizedExceptionCusotme => new ApiResponse<string>(ex.Message, 401),
                BadRequestExceptionCustome Br => new ApiResponse<string>(Br.Message, 400, Br._errors?.ToList()),
                _ => new ApiResponse<string>(serverErrorMessage, 500)
            };

            context.Response.StatusCode = response.StatusCode;
            response.IsSuccess = false;

            var json = JsonSerializer.Serialize(response
                , new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }
    }
}
