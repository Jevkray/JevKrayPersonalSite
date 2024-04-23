using System.Drawing;

namespace JevKrayPersonalSite.Services.ServiceInterfaces
{
    public interface ICaptchaService
    {
        Task<bool> CheckCaptchaAsync(string captcha);
        Task<(string, Bitmap)> CreateCaptchaAsync();
    }
}
