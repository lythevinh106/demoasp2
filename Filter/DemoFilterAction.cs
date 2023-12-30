using demoAsp2.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace demoAsp2.Filter
{//thuong dung bat du lieu dau vao
    public class DemoFilterAction : Attribute, IAsyncActionFilter
    {
        private string _email;

        public DemoFilterAction(string email)


        => _email = email;

        public async System.Threading.Tasks.Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Log.Information("day la actionfilter vào" + _email);
            var actionArguments = context.ActionArguments;










            foreach (var arg in actionArguments)
            {
                if (arg.Value is SignInModel model)
                {
                    var hasEmailField = model.GetType().GetProperties().Any(prop =>
                        string.Equals(prop.Name, "Email", StringComparison.InvariantCultureIgnoreCase));

                    if (hasEmailField)
                    {

                        var Request = context.HttpContext.Request;





                        using (var reader = new StreamReader(Request.Body))
                        {

                            string body = await reader.ReadToEndAsync();
                            Log.Information("day la body" + model.Email);
                            if (model.Email == _email)
                            {

                                Log.Information("day la actionfilter khach vip--- vào" + _email);
                            }
                            else
                            {
                                Log.Information("day la actionfilter khach binh thuong--- vào" + _email);
                            }



                        }




                    }
                }
            }

            await next();
            Log.Information("day la actionfilter tra ve" + _email);
        }


        /* public void OnActionExecuting(ActionExecutingContext context)
         {
             Log.Information("day la actionfilter vào" + _email);
             var actionArguments = context.ActionArguments;

             foreach (var arg in actionArguments)
             {
                 if (arg.Value is SignInModel model)
                 {
                     var hasEmailField = model.GetType().GetProperties().Any(prop =>
                         string.Equals(prop.Name, "Email", StringComparison.InvariantCultureIgnoreCase));

                     if (hasEmailField)
                     {
                         var Request = context.HttpContext.Request.Body;
                         using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                         {
                             var body = await reader.ReadToEndAsync();
                             if (body.Contains(_email))
                             {
                                 Log.Information("day la actionfilter khach vip--- vào" + _email);
                             }
                             else
                             {
                                 Log.Information("day la actionfilter khach vip--- vào" + _email);
                             }



                         }



                     }
                 }
             }



         }*/







    }


}
