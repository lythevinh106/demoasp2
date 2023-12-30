namespace demoAsp2.MiddleWare
{
    public class IntentionalDelayMiddleware
    {
        private readonly RequestDelegate _next;

        public IntentionalDelayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await Task.Delay(1000);

            await _next(context);

            await Task.Delay(1000);
        }
    }
}
