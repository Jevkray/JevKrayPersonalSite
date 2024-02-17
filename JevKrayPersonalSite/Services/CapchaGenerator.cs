﻿using Docker.DotNet.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection.PortableExecutable;

namespace JevKrayPersonalSite.Services
{
    internal class CapchaGenerator
    {

        public static async Task<(string, Bitmap)> CreateCapchaAsync()
        {
            string code = GenerateRandomText();
            Bitmap image = await GenerateImageAsync(code);

            return (code, image);
        }

        private static async Task<Bitmap> GenerateImageAsync(string code)
        {
            // Создаем новое изображение с размерами 200x100 пикселей
            Bitmap bmp = new Bitmap(200, 100);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // Задаем цвет фона
                graphics.Clear(Color.White);

                // Задаем цвет текста и шрифт
                using (Brush brush = new SolidBrush(Color.FromArgb(74, 86, 234)))
                using (Font font = new Font("Consolas", 24))
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
                    double intensity = random.Next(10, 15); // Случайная интенсивность искажения (волны или водоворота)

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
            int length = random.Next(6, 8); // Генерируем случайную длину от 6 до 7 символов
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789"; // Исключаем символы "O" и "0"
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
