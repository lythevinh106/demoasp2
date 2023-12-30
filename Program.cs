using demoAsp2.Authorize;
using demoAsp2.Constacts;
using demoAsp2.Data;
using demoAsp2.Enum;
using demoAsp2.Extensions;
using demoAsp2.Infrastructure;
using demoAsp2.Interfaces;
using demoAsp2.Logging;
using demoAsp2.Models;
using demoAsp2.Responsitory;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

namespace demoAsp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //var hangFireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {

            }).AddJsonOptions(x =>
               x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);




            //mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            ///Di
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddTransient<ILoggingService, LoggingService>();

            builder.Services.AddTransient<IAccountRepository, AccountRepository>();


            builder.Services.AddScoped<IMail, Mail>();



            builder.Services.AddScoped<IRepository<Product>, ProductGenericReponsitory>();
            builder.Services.AddScoped<IRepository<Order>, OrderGenericReponsitory>();


            //init unit of work cach trien khai 1 recomend

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //init unit of work cach trien khai 2
            builder.Services.AddScoped<IUnitOfWork2, UnitOfWork2>();

            ///the, razor page

            builder.Services.AddRazorPages();




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();











            ////cors
            //builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
            // policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            //db context
            builder.Services.AddDbContext<DBAspDemo2Context>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

                options.UseLazyLoadingProxies();


            });






            ///register identity
            ///


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DBAspDemo2Context>()
                .AddDefaultTokenProviders();


            ////


            ////identity option

            builder.Services.Configure<IdentityOptions>(
                options =>

                {
                    options.SignIn.RequireConfirmedEmail = true;

                    ///// Default Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    /////

                }

            );
            builder.Services.Configure<DataProtectionTokenProviderOptions>(
                op =>
                {

                    op.TokenLifespan = TimeSpan.FromHours(2);
                });


            ///register jwt
            builder.Services.AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


              }).AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.RequireHttpsMetadata = false;///thiet lap k can phai la https 
                  options.TokenValidationParameters = new
                  Microsoft.IdentityModel.Tokens.TokenValidationParameters

                  {
                      ValidateIssuer = true,/*k bắt buộc nó là iss():vd :https://localhost:7220
                                          Issuer không bắt buộc phải có, nhưng thường được khuyến khích để cung cấp
                                          thông tin về nơi mà token được tạo ra.*/


                      ValidateAudience = true,//k bắt buộc "aud": "user",
                                              //thương thì nó sẽ có ý nghĩa ai là người thất jwt này

                      ValidAudience = builder.Configuration["JWT:ValidAudience"],

                      ValidIssuer = builder.Configuration["JWT:ValidIssuer"],

                      /*
                       trong phần  chữ kí của  jwt có 3 phần
                      Signature (Chữ Ký):

                         HMACSHA256(
                         base64UrlEncode(header) + "." +
                         base64UrlEncode(payload),
                         your-256-bit-secret  --day là  IssuerSigningKey 
                         )



                       */
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(

                          builder.Configuration["JWT:Serect"]
                          ))


                  };


                  /*

                   rong JWT (JSON Web Token), "claim" là một phần quan trọng để chứa thông tin về người dùng 
                  hoặc về toke
                  Có hai loại claim chính:

                Reserved Claims: Các claim mà chuẩn JWT định rõ và có ý nghĩa chung. 
                  Một số reserved claims quan trọng bao gồm:

                iss (Issuer): Người tạo ra token.
                sub (Subject): Chủ thể của token, thường là id của người dùng.
                aud (Audience): Đối tượng mà token được tạo để sử dụng.
                exp (Expiration Time): Thời gian khi token sẽ hết hạn.
                nbf (Not Before): Thời gian trước khi token không hợp lệ.
                iat (Issued At): Thời gian khi token được tạo ra.
                jti (JWT ID): Một ID duy nhất cho token.

                Custom Claims: Những claim mà bạn tự định nghĩa và thêm vào token theo nhu cầu
                  của ứng dụng bạn. Điều này có thể bao gồm thông tin như tên người dùng, quyền lợi, thông tin tùy chỉnh, và nhiều điều khác nữa.
                   */
              }
              );






            ///add MediaTr
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());



            ///config json response
            builder.Services.AddControllers(
                //trien khai gloabal filter
                //se om tron cac filter controller
                //options => options.Filters.Add<DemoFilterAction>()
                )
              .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
             });

            //io



            ////ad background serice
            ///

            ///chỗ này là singleton vs background service
            /*   builder.Services.AddHostedService<BackgroundWorkerService>();


               builder.Services.AddScoped<IScopedService, MyScopedService>();
            */


            ///HangFire client

            //builder.Services.AddHangfire(configuration => configuration
            // .SetDataCompatibilityLevel(CompatibilityLevel.Version_180).
            // UseSimpleAssemblyNameTypeSerializer()
            //.UseRecommendedSerializerSettings().
            // UseSqlServerStorage(hangFireConnectionString)
            //  );
            ///HangFire client

            builder.Services.AddHangfire(configuration => configuration
     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
     .UseSimpleAssemblyNameTypeSerializer()
     .UseRecommendedSerializerSettings()
     .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));




            ///hangFire server
            builder.Services.AddHangfireServer();









            ////add azure service bus :
            builder.Services.AddAzureClients((clientBuilder) =>
            {
                var conectionString = "Endpoint=sb://azureservicebusdemo1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=TuRjqz9Fv9FSHx2UBwq5RmZaR/oZ316LF+ASbCCUCMA=";

                clientBuilder.AddServiceBusClient(conectionString).WithName("client1");





                var conectionString2 = "Endpoint=sb://demotopicsubnamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EBioRTP0KmzU8ACz13l7W46zAgmAz+cWf+ASbKrspYA=";

                clientBuilder.AddServiceBusClient(conectionString2).WithName("client2");



                var conectionStringBlob1 = "DefaultEndpointsProtocol=https;AccountName=storagepicture;AccountKey=i/P29BdqcelFFtgU9b34pkQtLjtR09WwHVuBUQT/IS1laMwHiouZF4wYYxZsQlL7gKsM33mTUqX1+AStAGTA1g==;EndpointSuffix=core.windows.net";

                clientBuilder.AddBlobServiceClient(conectionStringBlob1).WithName("blob1");


            });
            builder.Services.AddScoped<BlobResponsitory>();








            //Endpoint=sb://demotopicsub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MLUCC/EPSchSJkbrZvI1eHoWQtl69tPN7+ASbDyvtM4=








            ////policy


            builder.Services.AddAuthorization(options =>
            {
                ///policy vs role claim
                // options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer", "Admin"));


                options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer", "Admin")


                .RequireClaim("age", "18") //claim ở đây thường ở trong jwt lun  
                )

                ;


                ///check xem nguoi dung co tuoi ,cách  linh hoạt hơn
                ///
                /*
                options.AddPolicy("Premiumcontent", policy => policy.RequireAssertion(context =>





                 context.User.HasClaim(claim => claim.Type == "age" && int.Parse(claim.Value) >= 18)

                  || context.User.IsInRole("Admin")


                      )








                ); */



                ///check bang custom policy

                options.AddPolicy("AtLeast21", policy =>

                    policy.Requirements.Add(new MinimumAgeRequirement(21)));

                ;

                options.AddPolicy("MaleSex", policy =>

                  policy.Requirements.Add(new SexRequirement("female")));

                ;

                var models = typeof(ICrudTable).Assembly.GetTypes().Where(x => typeof(ICrudTable).IsAssignableFrom(x))
                               .Where(x => !x.IsInterface).ToList();


                foreach (var m in models)
                {


                    options.AddPolicy($"Create{m.Name}Policy",
                   policy => policy.Requirements.Add(new permissonCrud(m.Name, AppRoleClaim
                   .Create

                   )));
                    options.AddPolicy($"Read{m.Name}Policy",
                       policy => policy.Requirements.Add(new permissonCrud(m.Name, AppRoleClaim
                   .Read)));

                    options.AddPolicy($"Update{m.Name}Policy",
                      policy => policy.Requirements.Add(new permissonCrud(m.Name, AppRoleClaim
                   .Update)));

                    options.AddPolicy($"Delete{m.Name}Policy",
                     policy => policy.Requirements.Add(new permissonCrud(m.Name, AppRoleClaim
                   .Delete)));


                }



            });

            ///add DI of authrize was custom

            builder.Services.AddSingleton<IAuthorizationHandler, SexRequirementHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

            builder.Services.AddTransient<IAuthorizationHandler, crudHandlers>();




            ///logger with serilog cach1
          /*  Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.Console().

                WriteTo.File("log/myLog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ;*/


            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();


            builder.Host.UseSerilog();


            ///di middleware
            /// builder.Services.AddTransient<ExceptionHanderlingMiddlerWare>();


            var app = builder.Build();







            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();




            }



            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();



            app.UseAuthentication();
            app.UseAuthorization();




            //app.MapGet("/", () => "Helo World");

            ////middle ware
            /*

             Run được sử dụng khi bạn muốn thêm một Middleware đơn giản để xử lý yêu cầu và 
            không muốn chuyển điều khiển cho Middleware tiếp theo. Nó không gọi next()
            để chuyển đến Middleware tiếp theo trong pipeline. 
            Thông thường, bạn sẽ sử dụng Run để xử lý cuối cùng trong pipeline.
             */

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("welcome too asp middle ware");
            //});

            /*


             Phương thức Use:

            Use được sử dụng khi bạn muốn thêm Middleware và chủ động chuyển 
            đều khiển cho Middleware tiếp theo trong pipeline
            bằng cách gọi next(). 
            Điều này cho phép Middleware chạy và chuyển yêu cầu tới Middleware tiếp theo trong chuỗi.
             */
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("middleware thu 2");
            //    await context.Response.WriteAsync("middlewarethy 2");
            //    await next.Invoke();
            //});

            //app.Use(async (context, next) =>
            //{

            //    Console.WriteLine("middleware thu 3");
            //    await context.Response.WriteAsync("middlewarethy 3");

            //    await next.Invoke();
            //});





            //app.UseMiddleware<SimpleMiddleWare>();

            ///su dung phuong thuc mo rong  
            ///cach1: it xai
            //MiddleWareExtensions.UseSimpleResponseMiddleware(app);
            //cach2 :xai nhieu

            app.UseSimpleResponseMiddleware();


            app.UseLoggingMiddleware();
            app.UseIntentionalDelayMiddleware();
            app.UseTimeLoggingMiddleware();


            app.UseExceptionHanderlingMiddlerWare();



            app.MapControllers();



            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("ket thuc");
            //});


            ///hangfire
            app.UseHangfireDashboard();
            app.MapHangfireDashboard("/hangfire");

            //RecurringJob.AddOrUpdate(() => Console.WriteLine("hello from hang fire"), "* * * * *");


            app.Run();

        }

    }
}
