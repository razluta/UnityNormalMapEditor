using System.IO;
using UnityEngine;

namespace UnityNormalMapEditor.Editor
{
    public static class TextureUtilities
    {
        public enum TextureChannel
        {
            Red,
            Green,
            Blue
        }
        
        public static void SaveTextureToPath(Texture2D texture, string path)
        {
            var bytes = new byte[0];

            var extension = Path.GetExtension(path);

            switch (extension)
            {
                case ".png":
                    bytes = texture.EncodeToPNG();
                    break;
                case ".tga":
                    bytes = texture.EncodeToTGA();
                    break;
                case ".jpg":
                    bytes = texture.EncodeToJPG();
                    break;
            }

           System.IO.File.WriteAllBytes(path, bytes);
        }
        
        public static Texture2D GetRwTextureCopy(Texture2D sourceTexture)
        {
            var renderTexture = RenderTexture.GetTemporary(
                sourceTexture.width,
                sourceTexture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);
 
            Graphics.Blit(sourceTexture, renderTexture);
            var previous = RenderTexture.active;
            RenderTexture.active = renderTexture;
            var rwTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
            rwTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            rwTexture.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);
            return rwTexture;
        }
        
        public static Texture2D GetTextureWithInvertedChannel(Texture2D texture, TextureChannel textureChannel)
        {
            var textureRwCopy = GetRwTextureCopy(texture);
            var textureWidth = textureRwCopy.width;
            var textureHeight = textureRwCopy.height;
            var newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            for (var i = 0; i < textureWidth; i++)
            {
                for (var j = 0; j < textureHeight; j++)
                {
                    switch (textureChannel)
                    {
                        case TextureChannel.Red:
                            newTexture.SetPixel
                            (
                                i, j,
                                new Color(
                                    1 - textureRwCopy.GetPixel(i, j).r,
                                    textureRwCopy.GetPixel(i, j).g,
                                    textureRwCopy.GetPixel(i, j).b)
                            );
                            break;
                        
                        case TextureChannel.Green:
                            newTexture.SetPixel
                            (
                                i, j,
                                new Color(
                                    textureRwCopy.GetPixel(i, j).r,
                                    1 - textureRwCopy.GetPixel(i, j).g,
                                    textureRwCopy.GetPixel(i, j).b)
                            );
                            break;
                        
                        case TextureChannel.Blue:
                            newTexture.SetPixel
                            (
                                i, j,
                                new Color(
                                    textureRwCopy.GetPixel(i, j).r,
                                    textureRwCopy.GetPixel(i, j).g,
                                    1 - textureRwCopy.GetPixel(i, j).b)
                            );
                            break;
                    }
                }
            }
            
            return newTexture;
        }

        public static Texture2D RotateNormalClockwise(Texture2D texture)
        {
            var textureRwCopy = GetRwTextureCopy(texture);
            var textureWidth = textureRwCopy.width;
            var textureHeight = textureRwCopy.height;
            var newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            for (var i = 0; i < textureWidth; i++)
            {
                for (var j = 0; j < textureHeight; j++)
                {
                    newTexture.SetPixel
                    (
                        i, j,
                        new Color(
                            1 - textureRwCopy.GetPixel(i, j).g,
                            textureRwCopy.GetPixel(i, j).r,
                            textureRwCopy.GetPixel(i, j).b)
                    );
                }
            }

            return newTexture;
        }
        
        public static Texture2D RotateNormalCounterclockwise(Texture2D texture)
        {
            var textureRwCopy = GetRwTextureCopy(texture);
            var textureWidth = textureRwCopy.width;
            var textureHeight = textureRwCopy.height;
            var newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            for (var i = 0; i < textureWidth; i++)
            {
                for (var j = 0; j < textureHeight; j++)
                {
                    newTexture.SetPixel
                    (
                        i, j,
                        new Color(
                            textureRwCopy.GetPixel(i, j).g,
                            1 - textureRwCopy.GetPixel(i, j).r,
                            textureRwCopy.GetPixel(i, j).b)
                    );
                }
            }

            return newTexture;
        }
        
        public static Texture2D RotateNormal180(Texture2D texture)
        {
            var textureRwCopy = GetRwTextureCopy(texture);
            var textureWidth = textureRwCopy.width;
            var textureHeight = textureRwCopy.height;
            var newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            for (var i = 0; i < textureWidth; i++)
            {
                for (var j = 0; j < textureHeight; j++)
                {
                    newTexture.SetPixel
                    (
                        i, j,
                        new Color(
                            1 - textureRwCopy.GetPixel(i, j).g,
                            1 - textureRwCopy.GetPixel(i, j).r,
                            textureRwCopy.GetPixel(i, j).b)
                    );
                }
            }

            return newTexture;
        }
        
        public static Texture2D FlipNormalHorizontal(Texture2D texture)
        {
            var textureRwCopy = GetRwTextureCopy(texture);
            var textureWidth = textureRwCopy.width;
            var textureHeight = textureRwCopy.height;
            var newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            for (var i = 0; i < textureWidth; i++)
            {
                for (var j = 0; j < textureHeight; j++)
                {
                    newTexture.SetPixel
                    (
                        i, j,
                        new Color(
                            1 - textureRwCopy.GetPixel(i, j).r,
                            textureRwCopy.GetPixel(i, j).g,
                            textureRwCopy.GetPixel(i, j).b)
                    );
                }
            }

            return newTexture;
        }
        
        public static Texture2D FlipNormalVertical(Texture2D texture)
        {
            var textureRwCopy = GetRwTextureCopy(texture);
            var textureWidth = textureRwCopy.width;
            var textureHeight = textureRwCopy.height;
            var newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

            for (var i = 0; i < textureWidth; i++)
            {
                for (var j = 0; j < textureHeight; j++)
                {
                    newTexture.SetPixel
                    (
                        i, j,
                        new Color(
                            textureRwCopy.GetPixel(i, j).r,
                            1 - textureRwCopy.GetPixel(i, j).g,
                            textureRwCopy.GetPixel(i, j).b)
                    );
                }
            }

            return newTexture;
        }
        
    }
}