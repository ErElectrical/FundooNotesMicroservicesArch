using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace UserServices.Authorisation
{
    public class MsMq
    {
        private MessageQueue messagequeue = new MessageQueue();

        public void Sender(string token)
        {
            this.messagequeue.Path = @".\pivate$\Tokens";
            try
            {
                if (MessageQueue.Exists(this.messagequeue.Path))
                {
                    MessageQueue.Create(this.messagequeue.Path);

                }
                this.messagequeue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                //register the Method to the event 
                this.messagequeue.ReceiveCompleted += MessageQue_RecivedCompleted;
                this.messagequeue.Send(token);
                this.messagequeue.BeginReceive();
                this.messagequeue.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void MessageQue_RecivedCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var message = this.messagequeue.EndReceive(e.AsyncResult);
            string token = message.Body.ToString();
            try
            {
                MailMessage mailmessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("jonapifundooapp@gmail.com", "maa@355133"),
                    EnableSsl = true,
                };
                mailmessage.From = new MailAddress("jonapifundooapp@gmail.com", "maa@355133");

                mailmessage.To.Add(new MailAddress("jonapifundooapp@gmail.com"));
                mailmessage.Body = token;
                mailmessage.Subject = "FundooNote App reset link";
                smtpClient.Send(mailmessage);
            }
            catch(Exception)
            {
                throw;
            }
    }

    }
}
