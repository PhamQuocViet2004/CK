using System;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace YourProject.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }

        // POST: Mail/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string from, string to, string subject, string notes, HttpPostedFileBase attachment)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(from);   // Người gửi
                    mail.To.Add(to);                     // Người nhận
                    mail.Subject = subject;
                    mail.Body = notes;
                    mail.IsBodyHtml = true;

                    // Nếu có file đính kèm
                    if (attachment != null && attachment.ContentLength > 0)
                    {
                        var att = new Attachment(attachment.InputStream, Path.GetFileName(attachment.FileName));
                        mail.Attachments.Add(att);
                    }

                    // Dùng cấu hình trong web.config
                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(mail);
                    }
                }

                ViewBag.Message = "Gửi email thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi khi gửi: " + ex.Message;
            }

            return View();
        }
    }
}
