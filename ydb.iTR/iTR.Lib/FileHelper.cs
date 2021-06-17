using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.Services;
using System.Web;
using System.Drawing;

namespace iTR.Lib
{
    public class FileHelper
    {
        public static bool CheckFileType(string fileName, String[] allowedExtensions)
        {
            string ext = Path.GetExtension(fileName);
            bool fileOK = false;

            //判断用户选择的文件类型是否受限
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (ext == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }
            return fileOK;
        }

        public static bool UploadImage(string base64String, string path, string fileName, string pageID, string ownerID, Boolean thumbnail = true)
        {
            bool flag = false;
            try
            {
                byte[] fs = Convert.FromBase64String(base64String);
                //fileName = Guid.NewGuid().ToString().Replace("-", "") + "." + fileName.Split('.')[1];

                //获取上传案例图片路径
                string fullpath = System.Web.HttpContext.Current.Server.MapPath(path);
                //string fullpath = @"D:\Work\yaodaibao\Code\Release\Files";/// 调试用
                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }
                //定义并实例化一个内存流，以存放提交上来的字节数组。
                MemoryStream m = new MemoryStream(fs);
                //定义实际文件对象，保存上载的文件。
                FileStream f = new FileStream(fullpath + "\\" + fileName, FileMode.Create);
                //把内内存里的数据写入物理文件
                m.WriteTo(f);

                if (thumbnail)
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(m);
                    System.Drawing.Image img = GetThumbnail(originalImage, 80, 50);
                    img.Save(fullpath + "\\" + "T_" + fileName);
                    img = null;
                    originalImage = null;
                }
                m.Close();
                f.Close();
                f = null;
                m = null;

                string sql = "";

                sql = "INSERT INTO Attachments(FPageID,FOwnerID,FFileName,FPath)VALUES('" + pageID + "','" + ownerID + "','" + fileName + "','" + path + "')";
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                runner = null;
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        private static System.Drawing.Image GetThumbnail(System.Drawing.Image image, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            //从Bitmap创建一个System.Drawing.Graphics
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            //设置
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //下面这个也设成高质量
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //下面这个设成High
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //把原始图像绘制成上面所设置宽高的缩小图
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, height);

            gr.DrawImage(image, rectDestination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            return bmp;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="base64String">文件base64</param>
        /// <param name="path">保存路径</param>
        /// <param name="fileID">文件ID</param>
        /// <param name="sql">保存完成之后需要执行的sql</param>
        /// <param name="connectStr">连接字符串</param>
        /// <returns></returns>
        public static bool UploadFile(string base64String, string path, long fileID, string sql = "", string connectStr = "")
        {
            bool flag = false;
            try
            {
                byte[] fs = Convert.FromBase64String(base64String);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //定义并实例化一个内存流，以存放提交上来的字节数组。
                MemoryStream m = new MemoryStream(fs);
                //定义实际文件对象，保存上载的文件。
                FileStream f = new FileStream(path + "\\" + fileID, FileMode.Create);
                //把内存里的数据写入物理文件
                m.WriteTo(f);
                m.Close();
                f.Close();
                f = null;
                m = null;
                if (sql != "" && connectStr != "")
                {
                    SQLServerHelper runner = new SQLServerHelper(connectStr);
                    runner.ExecuteSqlNone(sql);
                }
                else if (sql != "" && connectStr == "")
                {
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 检查文件类型
        /// </summary>
        /// <param name="filename">上传文件名</param>
        public static void CheckFileType(string filename)
        {
            List<string> filterType = new List<string>() { "exe", "php", "bat", "asp", "jsp", "js", "vbs", "wsf", "wsh", "sh", "shell", "perl", "sql", "python", "ruby", "lua" };
            string[] name = filename.Split('.');
            if (filterType.Contains(name[name.Length - 1].ToLower()))
            {
                throw new Exception("上传文件失败，不允许上传此类型文件！");
            }
        }
    }
}