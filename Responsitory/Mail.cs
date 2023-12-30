using MailKit.Security;
using MimeKit;
using MimeKit.Text;


namespace demoAsp2.Responsitory
{


    public interface IMail
    {
        public Task<bool> SendMail(string subject, string body, string emailFrom,
            string emailTo
            ,
            string password
            );


        public void sendMail2();

    }

    public class Mail : IMail
    {







        public async Task<bool> SendMail(string subject, string body, string emailFrom,
            string emailTo
            ,
            string password
            )
        {

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(emailFrom));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body



            };



            using var smtp = new MailKit.Net.Smtp.SmtpClient();



            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailFrom, "dird zgsh hgzq bens");

            smtp.Send(email);

            smtp.Disconnect(true);
            return true;
        }

        public void sendMail2()
        {
            Console.WriteLine("Cong viec gui mail 1 da dc hoan thanh va chay gui mail 2");
        }
    }
}
