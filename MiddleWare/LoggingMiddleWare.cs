using demoAsp2.Logging;

namespace demoAsp2.MiddleWare
{
    public class LoggingMiddleWare
    {


        private readonly RequestDelegate _next;
        private readonly ILoggingService _logger;

        public LoggingMiddleWare(RequestDelegate next, ILoggingService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Console.WriteLine("custom middlewareeeeeeee11e1e2");
            //await _next.Invoke(context);
            //Console.WriteLine("du lieu dc response custom middleware2");



            _logger.Log(LogLevel.Information, context.Request.Path);

            //Invoke the next middleware in the pipeline
            await _next(context);

            //Get distinct response headers
            var uniqueResponseHeaders
                = context.Response.Headers
                                  .Select(x => x.Key)
                                  .Distinct();

            //Log these headers
            _logger.Log(LogLevel.Information, string.Join(", ", uniqueResponseHeaders));
        }

    }
}
