using JevKrayPersonalSite.DAL;
using JevKrayPersonalSite.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace JevKrayPersonalSite.Services
{

    public class CaptchaService : ICaptchaService
    {
        private readonly JevkSiteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CaptchaService(JevkSiteDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<bool> CheckCaptchaAsync(string capcha)
        {
            string sessionId = _httpContextAccessor.HttpContext.Request.Cookies["CapchaSessionId"];

            if (sessionId != null)
            {
                bool isValidCapcha = await IsValidCaptchaAsync(capcha.ToLower(), sessionId);

                if (isValidCapcha)
                {
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete("CapchaSessionId");

                    var capchaSession = await _dbContext.CapchaSessions.FirstOrDefaultAsync(c => c.SessionId == sessionId);

                    if (capchaSession != null)
                    {
                        _dbContext.CapchaSessions.Remove(capchaSession);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return isValidCapcha;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> IsValidCaptchaAsync(string capcha, string sessionId)
        {
            var capchaSessionModel = await _dbContext.CapchaSessions.FirstOrDefaultAsync(c => c.SessionId == sessionId);

            if (capchaSessionModel != null)
            {
                string inputCapchaHash = CacherService.CalculateMD5Hash(capcha);
                bool isValid = inputCapchaHash == capchaSessionModel.CapchaCache;

                return isValid;
            }
            else
            {
                return false;
            }
        }

        public async Task<(string, Bitmap)> CreateCaptchaAsync()
        {
            string code = GenerateRandomText().ToLower();
            Bitmap image = await GenerateImageAsync(code);

            return (code, image);
        }

#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        private static async Task<Bitmap> GenerateImageAsync(string code)
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        {
            // Создаем новое изображение с размерами 200x100 пикселей
            Bitmap bmp = new Bitmap(100, 50);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // Задаем цвет фона
                graphics.Clear(Color.FromArgb(56, 56, 56));

                // Задаем цвет текста и шрифт
                using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                using (Font font = new Font("Consolas", 25))
                {
                    // Определяем размеры текста
                    SizeF textSize = graphics.MeasureString(code, font);

                    // Определяем позицию текста для центрирования
                    float x = (bmp.Width - textSize.Width) / 2;
                    float y = (bmp.Height - textSize.Height) / 2;

                    // Отрисовываем текст на изображении
                    graphics.DrawString(code, font, brush, x, y);
                }

                Random random = new Random();
                using (var distortedImage = new Bitmap(bmp.Width, bmp.Height))
                using (Graphics distortedGraphics = Graphics.FromImage(distortedImage))
                {
                    distortedGraphics.Clear(Color.White);
                    distortedGraphics.SmoothingMode = SmoothingMode.HighQuality;

                    // Случайный выбор направления и интенсивности искажения
                    double angle = random.NextDouble() * 360; // Случайный угол для водоворота
                    double intensity = random.Next(10, 11); // Случайная интенсивность искажения (волны или водоворота)

                    for (int x = 0; x < bmp.Width; x++)
                    {
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            int offsetX = (int)(Math.Sin((x * Math.Cos(angle) + y * Math.Sin(angle)) * 0.05) * intensity);
                            int offsetY = (int)(Math.Cos((x * Math.Sin(angle) - y * Math.Cos(angle)) * 0.05) * intensity);
                            if (x + offsetX >= 0 && x + offsetX < bmp.Width && y + offsetY >= 0 && y + offsetY < bmp.Height)
                            {
                                distortedImage.SetPixel(x + offsetX, y + offsetY, bmp.GetPixel(x, y));
                            }
                        }
                    }

                    // Копируем искаженное изображение обратно в оригинальное
                    graphics.DrawImage(distortedImage, 0, 0);
                }
            }

            return bmp;
        }


        private static string GenerateRandomText()
        {
            Random random = new Random();
            int length = 3;//random.Next(6, 8); // Генерируем случайную длину от 6 до 7 символов
            const string chars = /*"abcdefghijkmnpqrstuvwxyz*/"0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}