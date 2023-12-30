using demoAsp2.MiddleWare;

namespace demoAsp2.Extensions
{
    public static class MiddleWareExtensions
    {
        ///cach 1 la truyen app vao
        //public static IApplicationBuilder UseSimpleResponseMiddleware(IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<SimpleMiddleWare>();
        //}

        //cacg2 thuong hay su dung
        public static IApplicationBuilder UseSimpleResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleMiddleWare>();
        }

        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleWare>();
        }

        public static IApplicationBuilder UseIntentionalDelayMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IntentionalDelayMiddleware>();
        }
        public static IApplicationBuilder UseTimeLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimeLoggingMiddleware>();
        }


        public static IApplicationBuilder UseExceptionHanderlingMiddlerWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHanderlingMiddlerWare>();
        }

        ///cach3:y chang cach 2 
        ///
        //public static void UseSimpleResponseMiddleware(this IApplicationBuilder builder)
        //{
        //    builder.UseMiddleware<SimpleMiddleWare>();
        //}

    }
}
