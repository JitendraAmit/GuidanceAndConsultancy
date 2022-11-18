using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Helpers
{
    public static class FileUploader
    {
        public static string UploadFile(string UploadDirectoryName, HttpPostedFileBase PostedFile)
        {
            try
            {
                string ErpDataFolder = AppConstants.ApplicationDataFolder;
                string strPathString = ErpDataFolder + "/" + UploadDirectoryName + "/";
                string FullDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("~/" + strPathString);

                if (!Directory.Exists(FullDirectoryPath))
                {
                    Directory.CreateDirectory(FullDirectoryPath);
                }

                string StrGuid = Guid.NewGuid().ToString();

                string FileExt = Path.GetExtension(PostedFile.FileName);
                string FileName = StrGuid;

                string CompleteFilePath = FullDirectoryPath + FileName + FileExt;
                PostedFile.SaveAs(CompleteFilePath);

                string ReturnPath = strPathString + FileName + FileExt; //save this path to database...

                return ReturnPath;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static string EditorUploadImages(string UploadDirectoryName, HttpPostedFileBase PostedFile)
        {
            try
            {
                string ErpDataFolder = AppConstants.ApplicationDataFolder;
                string strPathString = ErpDataFolder + "/" + UploadDirectoryName + "/";
                string FullDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("~/" + strPathString);

                if (!Directory.Exists(FullDirectoryPath))
                {
                    Directory.CreateDirectory(FullDirectoryPath);
                }

                string StrGuid = Guid.NewGuid().ToString();

                string FileExt = Path.GetExtension(PostedFile.FileName);
                string FileName = StrGuid;

                string CompleteFilePath = FullDirectoryPath + FileName + FileExt;
                PostedFile.SaveAs(CompleteFilePath);

                string ReturnPath = FileName + FileExt; //save this path to database...

                return ReturnPath;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static string UploadBase64StringToImage(string UploadDirectoryName, string Base64ImageString)
        {

            try
            {
                string ErpDataFolder = AppConstants.ApplicationDataFolder;
                string strPathString = ErpDataFolder + "/" + UploadDirectoryName + "/";
                string FullDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("~/" + strPathString);

                if (!Directory.Exists(FullDirectoryPath))
                {
                    Directory.CreateDirectory(FullDirectoryPath);
                }

                string StrGuid = Guid.NewGuid().ToString();

                // Convert base 64 string to byte[]
                byte[] imageBytes = Convert.FromBase64String(Base64ImageString.Replace("data:image/png;base64,", ""));

                System.Drawing.Image image;

                // Convert byte[] to Image
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    image = System.Drawing.Image.FromStream(ms, true);
                }

                string FileName = StrGuid;

                string CompleteFilePath = FullDirectoryPath + FileName + ".jpg";

                image.Save(CompleteFilePath);

                string ReturnPath = strPathString + FileName + ".jpg"; //save this path to database...

                return ReturnPath;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public static string Compressimage(Stream sourcePath, string UploadDirectoryName, String filename)
        {


            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 600.0f;
                    float maxWidth = 600.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    //const float resolution = 72;

                    //originalBMP.SetResolution(resolution, resolution);


                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }



                    string ErpDataFolder = AppConstants.ApplicationDataFolder;
                    string strPathString = ErpDataFolder + "/" + UploadDirectoryName + "/";
                    string FullDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("~/" + strPathString);

                    if (!Directory.Exists(FullDirectoryPath))
                    {
                        Directory.CreateDirectory(FullDirectoryPath);
                    }

                    string StrGuid = Guid.NewGuid().ToString() + "-" + Utilities.GetCurrentDateTime().ToString("yyyy-MM-dd") + "-" + Utilities.GetCurrentDateTime().ToString("hh-mm-ss");

                    string FileExt = Path.GetExtension(filename);
                    string FileName = StrGuid;

                    string CompleteFilePath = FullDirectoryPath + FileName + FileExt;


                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);

                    string ReturnPath = "";
                    extension = Path.GetExtension(CompleteFilePath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;    //SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.Low;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        //// try for png image compress code
                        //ImageCodecInfo pngEncoder = GetEncoder(ImageFormat.Png);
                        //Encoder myEncoder = Encoder.Quality;
                        //Encoder myEncoder2 = Encoder.Compression;
                        //EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 0L);   //50L
                        //myEncoderParameters.Param[0] = myEncoderParameter;
                        //imgGraph.Dispose();
                        //originalBMP.Dispose();
                        //bitMAP1.Save(CompleteFilePath, pngEncoder, myEncoderParameters);
                        //// try code end here......
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                        bitMAP1.Save(CompleteFilePath, image.RawFormat);

                        bitMAP1.Dispose();

                        ReturnPath = strPathString + FileName + FileExt; //save this path to database...

                    }
                    else if (extension == ".jpg" || extension ==".jpeg" )
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.Low;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                        bitMAP1.Save(CompleteFilePath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();

                        ReturnPath = strPathString + FileName + FileExt; //save this path to database...
                    }


                    

                    return ReturnPath;

                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }





        public static string UploadExcelImportFile(HttpPostedFile PostedFile)
        {
            try
            {
                string ErpDataFolder = AppConstants.ApplicationDataFolder;

                string StrGuid = Guid.NewGuid().ToString();

                string strPathString = ErpDataFolder + "/ExcelImportFiles/" + StrGuid + "/";

                string FullDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("~/" + strPathString);

                if (!Directory.Exists(FullDirectoryPath))
                {
                    Directory.CreateDirectory(FullDirectoryPath);
                }

                string CompleteFilePath = FullDirectoryPath + PostedFile.FileName;

                PostedFile.SaveAs(CompleteFilePath);

                //string ReturnPath = strPathString + PostedFile.FileName; //return this path...

                return CompleteFilePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}