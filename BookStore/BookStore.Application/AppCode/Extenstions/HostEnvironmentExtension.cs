using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace BookStore.Application.AppCode.Extenstions
{
    public static partial class Extension
    {
        static public string GetImagePhysicalPath(this IHostEnvironment env, string fileName)
        {
            return Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", fileName);
        }

        static public string GetImagePhysicalPath(this string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        static public void ArchiveImage(this IHostEnvironment env, string fileName)
        {
            var imageActualPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", fileName);

            if (File.Exists(imageActualPath))
            {
                var imageNewPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", $"archive-{DateTime.Now:yyyyMMddHHmmss}-{fileName}");

                File.Move(imageActualPath, imageNewPath);
            }
        }
    }
}
