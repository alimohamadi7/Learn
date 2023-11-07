using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Learn.Core.Convertors
{
    public static class CheckSizeImage
    {
        public static bool ImageSize(this IFormFile file)
        {
            //if (file.Length >= 140)
            //{
            //    return false;

            //}
            //return true;
            using (var image = Image.FromStream(file.OpenReadStream()))
            {
                if (image.Height >= 300 && image.Width >= 300)
                {

                    return false;
                }
                return true;
            }

        }
    }
}