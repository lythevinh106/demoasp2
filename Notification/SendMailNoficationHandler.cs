using MediatR;
using Serilog;

namespace demoAsp2.Notification
{
    public class SendMailNoficationHandler : INotificationHandler<SendMailNofication>
    {
        public Task Handle(SendMailNofication notification, CancellationToken cancellationToken)
        {
            Log.Information($"{notification.Message}----------------");



            return Task.CompletedTask;
        }
    }


    public class SendMailNoficationHandler2 : INotificationHandler<SendMailNofication>
    {
        public async Task Handle(SendMailNofication notification, CancellationToken cancellationToken)
        {
            Log.Information($"Demo send Mail-------------");

            await Task.Delay(5000, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                // Xử lý khi yêu cầu bị hủy bỏ trong quá trình xử lý thông báo
                Log.Information($"Nguoi dung huy send mail-------------");
                return;
            }






        }
    }
}
