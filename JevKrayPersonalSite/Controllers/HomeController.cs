using JevKrayPersonalSite.DAL;
using JevKrayPersonalSite.Models;
using JevKrayPersonalSite.Services;
using JevKrayPersonalSite.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace JevKrayPersonalSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICaptchaService _captchaService;
        private readonly JevkSiteDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, JevkSiteDbContext dbContext, ICaptchaService captchaService)
        {
            _captchaService = captchaService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Education()
        {
            return View();
        }

        public IActionResult Skills()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCaptcha()
        {
            var captchaResult = await _captchaService.CreateCaptchaAsync(); // Ваш метод для создания капчи

            // Преобразование изображения в массив байтов в формате PNG
            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                captchaResult.Item2.Save(ms, ImageFormat.Png);
                imageBytes = ms.ToArray();
            }

            string capchaCache = CacherService.CalculateMD5Hash(captchaResult.Item1);
            string sessionId = Guid.NewGuid().ToString();

            CapchaSessionModel capchaSessionModel = new CapchaSessionModel()
            {
                SessionId = sessionId,
                CapchaCache = capchaCache,
                CreatedAt = DateTime.Now,
            };

            _dbContext.Add(capchaSessionModel);
            await _dbContext.SaveChangesAsync();

            Response.Cookies.Append("CapchaSessionId", sessionId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(5),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return File(imageBytes, "image/png");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}