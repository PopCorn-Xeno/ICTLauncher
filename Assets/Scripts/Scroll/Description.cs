using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ICTLauncher.Scroll
{
    class Description
    {
        public string title;
        public string author;
        public string[] comment;
        public string[] control;
        public string applicationFilename;
        public string iconFilename;
        public Sprite iconData = null;
        public string ApplicationPath => applicationFilename != null
                                            ? $"{DirectoryName}/{applicationFilename}".Replace(@"\", "/")
                                            : throw new NullReferenceException("A property `applicationFilename` is null.\nCheck description.json.");
        public string IconPath => iconFilename != null
                                    ? $"{DirectoryName}/{iconFilename}".Replace(@"\", "/")
                                    : throw new NullReferenceException("A property `iconFilename` is null.\nCheck description.json.");
        public string DirectoryName { private get; set; }

        public void OpenApplication()
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(applicationFilename, @"https?://[\w!?/+\-_~;.,*&@#$%()'[\]]+"))
            {
                Application.OpenURL(new Uri(applicationFilename).AbsoluteUri);
                return;
            }
            if (File.Exists(ApplicationPath))
            {
                System.Diagnostics.Process.Start(ApplicationPath);
            }
            throw new FileNotFoundException($"{ApplicationPath} does not exist.");
        }

        public void GetIcon()
        {
            if (iconFilename != null)
            {
                if (File.Exists(IconPath))
                {
                    byte[] raw = File.ReadAllBytes(IconPath);
                    Texture2D texture = new(0, 0);
                    texture.LoadImage(raw);
                    iconData = Sprite.Create(texture, new (0f, 0f, texture.width, texture.height), new (0.5f, 0.5f));
                }
                else throw new FileNotFoundException($"{ApplicationPath} does not exist.");
            }
            else
            {
                iconData = Resources.Load<Sprite>("Images/icon_file_not_found");
                #if UNITY_EDITOR
                Debug.LogWarning($"Alternate icon was set.");
                #endif
            }
        }
        /*
        public void GetIcon()
        {
            if (iconFilename == null)
            {
                if (File.Exists(ApplicationPath))
                {
                    Debug.Log(ApplicationPath);
                    Icon icon = Icon.ExtractAssociatedIcon(ApplicationPath);
                    
                    if (icon != null)
                    {
                        using Bitmap bitmap = icon.ToBitmap();

                        Texture2D texture = new (bitmap.Width, bitmap.Height, TextureFormat.RGBA32, false);
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x++)
                            {
                                System.Drawing.Color color = bitmap.GetPixel(x, y);
                                texture.SetPixel(x, bitmap.Height - 1 - y, new (color.R, color.G, color.B, color.A));
                            }
                        }
                        texture.Apply();
                        textureData = texture;
                        iconData = Sprite.Create(texture, new (0, 0, texture.width, texture.height), new (0.5f, 0.5f));
                        // return iconData;
                    }
                    throw new Exception("Application icon could not get.");
                    
                }
                throw new FileNotFoundException($"{ApplicationPath} does not exist.");
            }
            throw new NullReferenceException("A property `icon` is null.");
        }
         */
    }

    public static class Extentions
    {
        public static string Listformat(this string[] text, string decorater = "・") => string.Join("\n", text.Select((line) => decorater + line));
    }
}