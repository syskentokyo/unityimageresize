using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Drawing;
using Graphics = UnityEngine.Graphics;

namespace SyskenTLib.ManualImageResize.Editor.SyskenTLib.ManualImageResize.Editor
{
    public class MainMenuWindow : EditorWindow
    {
        
        [MenuItem("SyskenTLib/ManualImage/Resize - Max512",priority = 30)]
        private static void ResizeMax512()
        {
            MainRezieProgress(512);

        }
        
        [MenuItem("SyskenTLib/ManualImage/Resize - Max1024",priority = 30)]
        private static void ResizeMax1024()
        {
            MainRezieProgress(1024);

        }
        
        [MenuItem("SyskenTLib/ManualImage/Resize - Max2048",priority = 30)]
        private static void ResizeMax2048()
        {
            MainRezieProgress(2048);

        }
        
        [MenuItem("SyskenTLib/ManualImage/Resize - Max4096",priority = 30)]
        private static void ResizeMax4096()
        {
            MainRezieProgress(4096);

        }


        private static void MainRezieProgress(int maxSize)
        {
            //フォルダ選択
            var selectDirpath = EditorUtility.OpenFolderPanel("Target Root Directory Max "+maxSize,  Application.dataPath, string.Empty);
            if (string.IsNullOrEmpty(selectDirpath))
                return;
            
            
            //画像ファイルのパス抽出
            List<string> fileAllPathList = Directory.GetFiles(selectDirpath, "*.*", SearchOption.AllDirectories).ToList()
                .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".tga")|| s.EndsWith(".exr")).ToList();

            fileAllPathList.ForEach(imageFilePath =>
            {
                using (Image image = Image.FromFile(imageFilePath))
                {
                    int srcSizeWidth = image.Width;
                    int srcSizeHeight = image.Height;
                    int targetSizeWidth = image.Width;
                    int targetSizeHeight = image.Height;

                    if (targetSizeWidth < maxSize && targetSizeHeight < maxSize)
                    {
                        //小さいサイズだった場合
                        
                        //とくになにもしない
                        
                    }else if (targetSizeWidth < targetSizeHeight)
                    {
                        // 大きな画像
                        //縦長の画像だった場合
                        targetSizeHeight = maxSize;
                        targetSizeWidth = srcSizeWidth * (maxSize / srcSizeHeight);

                        //リサイズと保存
                        ResizeAndSaveToFile(imageFilePath
                            , srcSizeWidth
                            , srcSizeHeight
                            , targetSizeWidth
                            , targetSizeHeight);
                    }
                    else
                    {
                        // 大きな画像
                        //横長の画像だった場合
                        targetSizeHeight = srcSizeHeight * (maxSize / srcSizeWidth);
                        targetSizeWidth = maxSize;

                        //リサイズと保存
                        ResizeAndSaveToFile(imageFilePath
                            , srcSizeWidth
                            , srcSizeHeight
                            , targetSizeWidth
                            , targetSizeHeight);
                        
                    }
                    
                    
                }
            });
            
        }
        
        
        private static Texture2D ResizeTexture(Texture2D srcTexture, int newWidth, int newHeight) {
            Texture2D newTexture = new Texture2D(newWidth, newHeight);
            Graphics.ConvertTexture(srcTexture, newTexture);
            return newTexture;
        }

        private static void ResizeAndSaveToFile(string filePath,int srcWidth, int srcHeight, int newWidth, int newHeight)
        {
            byte[] newTexutreBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(srcWidth, srcHeight);
            texture.LoadImage(newTexutreBytes);

            Texture2D newTexture = ResizeTexture(texture, newWidth, newHeight);

            if (Path.GetExtension(filePath) == ".jpg")
            {
               byte[] saveImageBytes = newTexture.EncodeToJPG(100);
               File.WriteAllBytes(filePath,saveImageBytes);
            }
            else if (Path.GetExtension(filePath) == ".png")
            {
                byte[] saveImageBytes = newTexture.EncodeToPNG();
                File.WriteAllBytes(filePath,saveImageBytes);
            }
            else if (Path.GetExtension(filePath) == ".tga")
            {
                byte[] saveImageBytes = newTexture.EncodeToTGA();
                File.WriteAllBytes(filePath,saveImageBytes);
            }
            else if (Path.GetExtension(filePath) == ".exr")
            {
                byte[] saveImageBytes = newTexture.EncodeToEXR();
                File.WriteAllBytes(filePath,saveImageBytes);
            }

            

        }

    }
}