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
            } catch (Exception ex)
            {
                switch (ex)
                {
                    case BadRequestException:
                        {
                            context.Response.StatusCode = BadRequestException.StatusCode;
                            break;
                        }
                    case ForbiddenException:
                        {
                            context.Response.StatusCode = ForbiddenException.StatusCode;
                            break;
                        }
                    case NotFoundException:
                        {
                            context.Response.StatusCode = NotFoundException.StatusCode;
                            break;
                        }
                    case UnauthorizedException:
                        {
                            context.Response.StatusCode = UnauthorizedException.StatusCode;
                            break;
                        }
                    default:
                        {
                            context.Response.StatusCode = 500;
                            break;
                        }
                }
                context.Response.ContentType = "application/json";
                if (context.Response.StatusCode != 500)
                {
                    
                    await context.Response.WriteAsJsonAsync(new { message = ex.Message });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new { 
                    message = ex.Message,
                    everything = ex.ToString()});  //for devolopment
                    //await context.Response.WriteAsJsonAsync(new { message = "Internal error, status code 500" }); //for production
                }
            }

        }
    }
}
