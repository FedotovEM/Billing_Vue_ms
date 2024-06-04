namespace AuthService.Helpers
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, Origin, Authorization, X-Requested-With");

            if (context.Request.Method == "OPTIONS")
            {
                await context.Response.WriteAsync("OPTIONS request");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
