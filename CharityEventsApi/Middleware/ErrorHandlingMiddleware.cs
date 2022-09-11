using CharityEventsApi.Exceptions;
namespace CharityEventsApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        { 
            try
            {
               await next.Invoke(context);
            }
            catch (BadRequestException bre)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;

                await context.Response.WriteAsync(bre.Message);
            }
            catch (NotFoundException nfe)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 404;

                await context.Response.WriteAsync(nfe.Message);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
