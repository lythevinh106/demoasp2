namespace demoAsp2
{
    public class BackgroundWorkerService : BackgroundService
    {


        private readonly IServiceProvider _provider;
        private readonly MyScopedService _myScopedService;
        readonly ILogger<BackgroundWorkerService> _logger;

        private int count;
        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger,
            IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;

        }

        ///doan nay se chay dau tien 
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("backgournd service StartAsync----");
            _logger.LogInformation("backgournd service StartAsync----");


            return base.StartAsync(cancellationToken);


        }
        ///ham nay se chay sau khi tat chuong trinh
        public override Task StopAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("backgournd service StopAsync---" + DateTimeOffset.Now);
            Console.WriteLine("backgournd service StopAsync---" + DateTimeOffset.Now);

            return base.StopAsync(cancellationToken);

        }

        ///ham nay se chay sau start async 
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {




            Console.WriteLine("ExecuteAsync service started------------- ");




            while (!stoppingToken.IsCancellationRequested)
            {
                count++;
                //vi 
                using (var scope = _provider.CreateScope())
                {


                    Console.WriteLine("ExecuteAsync service workingg-------------" + count + "---" + DateTimeOffset.Now);
                    var scopService = scope.ServiceProvider.GetRequiredService<IScopedService>();
                    scopService.Write();

                    await Task.Delay(1000, stoppingToken);//stoppingToken ở đây đảm bảo là công việc
                                                          //chạy nền sẽ dừng ngay lập tức khi  mà chương trình dừng


                }

            }



        }
    }
}