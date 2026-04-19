namespace DapperOnlineStoreAPI.GlobalExceptionHandler
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed: {Errors}", ex.Errors);

                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    errors = ex.Errors,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "System Exception");

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message,
                    type = ex.GetType().Name
                });
            
        }
        }
    }          
}
