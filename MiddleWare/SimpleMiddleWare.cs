namespace demoAsp2.MiddleWare
{
    public class SimpleMiddleWare
    {
        private readonly RequestDelegate next;
        public SimpleMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            Console.WriteLine("custom middlewareeeeeeee11e1e1");
            await next.Invoke(context);
            Console.WriteLine("du lieu dc response custom middleware1");
        }


    }
}
