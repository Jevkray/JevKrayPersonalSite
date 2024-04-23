using JevKrayPersonalSite.PrivateServices.MailSender;
using JevKrayPersonalSite.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace JevKrayPersonalSite.Controllers.ApiControllers
{
    public class MailApiController : Controller
    {
        private readonly ICaptchaService _captchaService;

        public MailApiController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        [HttpPost]
        public async Task<IActionResult> MailSender(string Username, string Email, string Title, string Message, string Capcha)
        {
            if (!await _captchaService.CheckCaptchaAsync(Capcha))
            {
                return BadRequest("Invalid Captcha");
            }
            else
            {
                var mail = CodeMail.CreateMail(Username, Email, Title, Message);
                CodeMail.SendMail(mail);
                return Ok("Mail Sent");
            }
        }
    }
}
