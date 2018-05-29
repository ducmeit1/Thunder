using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Thunder.Droid.IServices;
using Thunder.IServices;
using Xamarin.Forms;
[assembly: Dependency(typeof(HelpersAndroid))]
namespace Thunder.Droid.IServices
{
    public class HelpersAndroid : IHelper
    {
        public async Task<bool> SendEmailAsync(string to, string title, string message)
        {
            string senderEmail = "ducmeitks2017@gmail.com";
            string senderPassword = "Ducpro123";
            string senderName = "Thunder Market";

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            mimeMessage.To.Add(new MailboxAddress(to));
            mimeMessage.Subject = title;
            mimeMessage.Body = new TextPart("html")
            {
                Text = message
            };
            var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect("smtp.gmail.com", 587, false);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(senderEmail, senderPassword);
            try
            {
                await client.SendAsync(mimeMessage);
                client.Disconnect(true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Send email is failed! Will try send again after 5 seconds, error message: " + ex.Message);
                await Task.Delay(5000);
                try
                {
                    await client.SendAsync(mimeMessage);
                    client.Disconnect(true);
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine("Send email is failed!" + ex1.Message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }

            return false;
        }
    }
}