using EFExample.Models;
using System.Net;
using System.Net.Mail;

namespace EFExample.Email
{
    public class EmailService
    {

        public void SendEmail(string ReceiverEmail, string Type, string ReceiverName, string SenderName)
        {
            try
            {
                string Body = null;

                if (Type == "newUser")
                {
                    Body = $"Hi {ReceiverName} welcome to the NoteBook";
                }
                else if (Type == "newPost")
                {
                    Body = $"Hi {ReceiverName} Your Friend {SenderName} Posted New Post";
                }
                else if(Type == "postShare")
                {
                    Body = $"Hi {ReceiverName} Your Friend {SenderName} Shared Your Post";
                }
                else if (Type == "newComment")
                {
                    Body = $"Hi {ReceiverName} Your Friend {SenderName} Commented On Your Post";
                }
                else if (Type == "likes")
                {
                    Body = $"Hi {ReceiverName} Your Friend {SenderName} likes you Post/comment/reply";
                }



                MailMessage message = new MailMessage();

                message.From = new MailAddress("gudla.naveen1998@gmail.com");
                message.To.Add(new MailAddress(ReceiverEmail));
                message.Subject = "Notification";
                message.IsBodyHtml = true;
                message.Body = Body;

                SmtpClient smtp = new SmtpClient();

                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("gudla.naveen1998@gmail.com", "eruaygepwnfstnmw");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }

            catch (Exception) { }

        }
    }
}