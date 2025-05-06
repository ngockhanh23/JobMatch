using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace JobMatch.Utils.MailUtils
{
    public class MailUtils
    {
        public static async Task SendMail(string from, string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart("html") { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(from, "vsci bbdf yomz vbae"); // Thay bằng app password
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email.");
                Console.WriteLine($"Error: {ex.Message}");
                throw; // hoặc return false, tuỳ bạn
            }
        }

        public static string GetRejectionEmailBody(string jobTitle, string companyName)
        {
            return $@"
            <h2>Đơn ứng tuyển của bạn đã bị từ chối</h2>
            <p>Chào bạn,</p>
            <p>Cảm ơn bạn đã ứng tuyển vào vị trí <strong>{jobTitle}</strong> tại <strong>{companyName}</strong>.</p>
            <p>Sau khi xem xét, chúng tôi rất tiếc phải thông báo rằng bạn chưa phù hợp với vị trí này ở thời điểm hiện tại.</p>
            <p>Chúc bạn sớm tìm được công việc phù hợp!</p>
            <p>Trân trọng,<br/><strong>{companyName}</strong></p>";
        }
    }
}
